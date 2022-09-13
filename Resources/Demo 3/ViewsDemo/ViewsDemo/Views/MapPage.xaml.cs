using System;
using ViewsDemo.ViewModels;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ViewsDemo.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MapPage : ContentPage
    {
        MapViewModel mapViewModel = new MapViewModel();
        public MapPage()
        {
            InitializeComponent();
            BindingContext = mapViewModel;

            mapViewModel.Map = this.map;
            mapViewModel.CenterToHeadQuarters();
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            Navigation.ShowPopup(new MapTypePage(mapViewModel));
        }
    }
}