using System;
using System.Diagnostics;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppEvents
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            this.ModalPopping += AppModalPopping;

            MainPage = new NavigationPage(new MainPage());
        }
        private void AppModalPopping(object sender, ModalPoppingEventArgs e)
        {
            Debug.WriteLine("App: " + e.Modal.GetType().Name, "PS");
        }

        protected override void OnStart()
        {
            Debug.WriteLine("OnStart called", "PS");
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
