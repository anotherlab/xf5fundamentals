using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using DailyForecast.ViewModels;

namespace DailyForecast
{
    public partial class MainPage : ContentPage
    {
        readonly DailyForecastViewModel ViewModel;
        public MainPage()
        {
            InitializeComponent();

            BindingContext = ViewModel = new DailyForecastViewModel();

            // Quick way to see if the current location is in a region that uses the metric system
            var myRegionInfo = new System.Globalization.RegionInfo(System.Globalization.CultureInfo.CurrentCulture.Name);

            ViewModel.IsMetric = myRegionInfo.IsMetric;
            ViewModel.ApiKey = APIKeys.API_KEY;
            ViewModel.Address = "Draper, UT, USA";
        }
    }
}
