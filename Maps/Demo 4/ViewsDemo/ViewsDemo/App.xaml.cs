using System;
using System.Linq;
using System.Threading.Tasks;
using ViewsDemo.Interfaces;
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

        MyLocationDataStore myLocationDataStore = null;
        public MyLocationDataStore MyLocationDataStore => myLocationDataStore ?? (myLocationDataStore = new MyLocationDataStore());

        readonly INotificationManager notificationManager;

        public ThemeViewModel ThemeViewModel { get; private set; }

        public App()
        {
            InitializeComponent();

            ThemeViewModel = new ThemeViewModel();

            // When the user changes the default theme on the device, we update the statusbar color on Android
            if (Device.RuntimePlatform == Device.Android)
            {
                Application.Current.RequestedThemeChanged += (o, e) =>
                    Device.BeginInvokeOnMainThread(() => ThemeViewModel.UpdateStatusBarColor());
            }

            MainPage = new AppShell();

            notificationManager = DependencyService.Get<INotificationManager>();

            notificationManager.NotificationReceived += (sender, eventArgs) =>
            {
                var e = (NotificationEventArgs)eventArgs;

                Device.BeginInvokeOnMainThread(async () =>
                {
                    await App.Current.MainPage.DisplayAlert("Alert", $"Notification Received:\nTitle: {e.Title}\nMessage: {e.Message}\nID: {e.Id}", "Ok");
                });

            };

            MessagingCenter.Subscribe<LeadersViewModel, string>(this, MessageConsts.RequestMeeting, async (sender, LeaderId) =>
            {
                await Task.Delay(TimeSpan.FromSeconds(2));

                var leader = await LeadersDataStore.GetItemAsync(LeaderId);

                await App.Current.MainPage.DisplayAlert("Message received", $"Meeting request submitted for {leader.Name}", "OK");
            });

        }

        public void SendNotification(string title, string message, string id)
        {
            notificationManager.SendNotification(title, message, id);
        }

        protected override void OnStart() { }

        protected override void OnSleep() { }

        protected override void OnResume() { }
    }
}
