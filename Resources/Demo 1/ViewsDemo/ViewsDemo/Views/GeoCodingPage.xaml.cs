using System;
using ViewsDemo.ViewModels;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;

namespace ViewsDemo.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GeoCodingPage : ContentPage
    {
        MapViewModel mapViewModel = new MapViewModel();
        public GeoCodingPage()
        {
            InitializeComponent();
            BindingContext = mapViewModel;

            mapViewModel.Map = this.map;
            mapViewModel.CenterToHeadQuarters();

            this.map.MapClicked += async (sender, e) =>
            {
                var info = await mapViewModel.ReverseGeoCoding(e.Position.Latitude, e.Position.Longitude);

                await DisplayAlert("Placemark", info, "Ok");
            };
        }
        private void Button_Clicked(object sender, EventArgs e)
        {
            Navigation.ShowPopup(new MapTypePage(mapViewModel));
        }
    }
}