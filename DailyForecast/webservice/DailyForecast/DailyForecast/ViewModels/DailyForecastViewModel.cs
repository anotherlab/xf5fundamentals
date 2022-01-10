using System;
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

        void UpdateDailyRange(List<DisplayedTemperature> DisplayedTemperatures)
        {
            var list = DisplayedTemperatures.ToList();

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

                UpdateDailyRange(DisplayedTemperatures);
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
        }
    }
}
