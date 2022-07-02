using ShellPages.Views;
using System;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ShellPages
{
    public partial class App : Application
    {
        Shell flyoutShell = null;
        public Shell FlyoutShell => flyoutShell ?? (flyoutShell = new AppShell());

        Shell tabShell = null;
        public Shell TabShell => tabShell ?? (tabShell = new TabShell());

        public App()
        {
            InitializeComponent();

            // Start tracking version information
            // https://docs.microsoft.com/en-us/xamarin/essentials/version-tracking?context=xamarin%2Fxamarin-forms
            VersionTracking.Track();

            MainPage = FlyoutShell;
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
