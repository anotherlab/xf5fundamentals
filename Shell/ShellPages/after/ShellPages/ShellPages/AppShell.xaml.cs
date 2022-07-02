using ShellPages.ViewModels;
using ShellPages.Views;
using System;
using System.Collections.Generic;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace ShellPages
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
        }

        private async void OnLoginClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//LoginPage");
        }

        private async void OnVersionClicked(object sender, EventArgs e)
        {
            await DisplayAlert("Version", VersionTracking.CurrentVersion, "OK");

            // Close the menu
            Shell.Current.FlyoutIsPresented = false;
        }
        private void OnTabsClicked(object sender, EventArgs e)
        {
            // Close the menu
            Shell.Current.FlyoutIsPresented = false;
            Application.Current.MainPage = (App.Current as App).TabShell;
        }

        protected override void OnNavigating(ShellNavigatingEventArgs args)
        {
            base.OnNavigating(args);
            var route = args.Target.Location.OriginalString.ToLower();

            // We are using a static property
            vmProperty.Parameter = route;
        }
    }
}
