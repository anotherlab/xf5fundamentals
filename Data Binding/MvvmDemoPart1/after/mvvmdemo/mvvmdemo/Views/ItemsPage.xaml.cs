using mvvmdemo.Models;
using mvvmdemo.ViewModels;
using mvvmdemo.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace mvvmdemo.Views
{
    public partial class ItemsPage : ContentPage
    {
        ItemsViewModel _viewModel;

        public ItemsPage()
        {
            InitializeComponent();

            BindingContext = _viewModel = new ItemsViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();

            //_ = CheckLoginStatus();
        }

        //private async Task CheckLoginStatus()
        //{
        //    if (!(App.Current as App).user.IsLoggedIn)
        //    {
        //        await Shell.Current.GoToAsync("//LoginPage");
        //    }
        //}
    }
}