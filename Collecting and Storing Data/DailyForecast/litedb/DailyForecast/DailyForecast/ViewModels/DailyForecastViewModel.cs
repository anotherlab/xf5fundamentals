﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using DailyForecast.Services;
using DailyForecast.Models;
using Xamarin.Forms;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Linq;


namespace DailyForecast.ViewModels
{
    public class DailyForecastViewModel : BaseViewModel
    {
        ForecastData _ForecastData = null;
        ForecastData ForecastData
        {
            get
            {
                if (_ForecastData == null)
                {
                    _ForecastData = new ForecastData();
                }

                return _ForecastData;
            }
        }
        ForecastDatabaseLiteDB dbLiteDB = null;

        Dictionary<DateTime, int> quickLookup = new Dictionary<DateTime, int>();

        public string ApiKey
        {
            get { return ForecastData.ApiKey; }
            set { ForecastData.ApiKey = value; }
        }

        bool _IsMetric;
        public bool IsMetric
        {
            get { return _IsMetric; }
            set
            {
                if (_IsMetric != value)
                {
                    _IsMetric = value;
                    ForecastData.Units = _IsMetric ? ForecastUnits.metric : ForecastUnits.imperial;
                }
            }
        }

        string _Address;
        public string Address
        {
            get { return _Address; }
            set { SetProperty(ref _Address, value); }
        }

        public ObservableCollection<DailyRange> DailyRanges { get; set; }

        List<DisplayedTemperature> ReshapeData(owmList[] list)
        {
            var DisplayedTemperatures = new List<DisplayedTemperature>();

            for (var i = 0; i < list.Length; i++)
            {
                var l = list[i];
                var displayedTemperature = new DisplayedTemperature
                {
                    ForecastDate = l.LocalTime,
                    TempMax = l.main.temp_max,
                    TempMin = l.main.temp_min
                };


                DisplayedTemperatures.Add(displayedTemperature);
            }

            return DisplayedTemperatures;
        }

        async Task InitLiteDB()
        {
            if (dbLiteDB == null)
            {
                dbLiteDB = await ForecastDatabaseLiteDB.Instance;
                dbLiteDB.PurgeOlder(10);
            }
            quickLookup = new Dictionary<DateTime, int>();

            var recs = dbLiteDB.GetAll().Select(x => new { x.ForecastDate, x.Id }).ToList();

            recs.ForEach(x => quickLookup.Add(x.ForecastDate, x.Id));
        }

        void UpdateLiteDBRows(List<DisplayedTemperature> DisplayedTemperatures)
        {
            dbLiteDB.BeginTrans();

            for (var i = 0; i < DisplayedTemperatures.Count; i++)
            {
                var displayedTemperature = DisplayedTemperatures[i];

                int id;
                if (quickLookup.TryGetValue(displayedTemperature.ForecastDate, out id))
                {
                    displayedTemperature.Id = id;
                }
                dbLiteDB.Upsert(displayedTemperature);
            }

            dbLiteDB.Commit();
        }

        async Task LoadData(bool DoInit = true)
        {
            if (DoInit)
            {
                await InitLiteDB();
            }

            var list = dbLiteDB.GetAll();

            var DailyRangeList = list.Select(x => new { x.ForecastDate.Date, x.TempMin, x.TempMax })
                                        .GroupBy(x => x.Date,
                                        (Date, forecastData) => new DailyRange
                                        {
                                            ForecastDate = Date,
                                            LowTemp = forecastData.Min(x => x.TempMin),
                                            HighTemp = forecastData.Max(x => x.TempMax)
                                        })
                                        .OrderBy(x => x.ForecastDate)
                                        .ToList();

            DailyRanges.Clear();
            DailyRangeList.ForEach(x => DailyRanges.Add(x));
        }

        async Task UpdateDailyRange(List<DisplayedTemperature> DisplayedTemperatures)
        {
            await InitLiteDB();

            UpdateLiteDBRows(DisplayedTemperatures);

            await LoadData(false);
        }

        public Command RefreshCommand { get; set; }

        private bool CanExecuteRefresh()
        {
            return !IsBusy;
        }

        private async Task ExecuteRefreshCommand()
        {
            IsBusy = true;

            try
            {
                var daily = await ForecastData.DailyForecast(Address);

                var DisplayedTemperatures = ReshapeData(daily.list);

                await UpdateDailyRange(DisplayedTemperatures);
            }
            catch (Exception ex)
            {

                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        public DailyForecastViewModel()
        {
            DailyRanges = new ObservableCollection<DailyRange>();

            RefreshCommand = new Command(async () => await ExecuteRefreshCommand(), CanExecuteRefresh);

            Task.Run(async () => { await LoadData(); });
        }
    }
}
