using System;
using System.Linq;
using System.Threading.Tasks;
using ViewsDemo.Services;
using ViewsDemo.ViewModels;
using ViewsDemo.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ViewsDemo
{
    public partial class App : Application
    {
        LeadersDataStore leadersDataStore = null;
        public LeadersDataStore LeadersDataStore => leadersDataStore ?? (leadersDataStore = new LeadersDataStore());

        public App()
        {
            InitializeComponent();
            MainPage = new AppShell();
        }

        protected override void OnStart() { }

        protected override void OnSleep() { }

        protected override void OnResume() { }
    }
}
