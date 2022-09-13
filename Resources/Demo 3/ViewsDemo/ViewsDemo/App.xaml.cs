using System;
using System.Linq;
using System.Threading.Tasks;
using ViewsDemo.Interfaces;
using ViewsDemo.Resources;
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

        private void ChangeCulture(string locale)
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(locale);
            System.Threading.Thread.CurrentThread.CurrentUICulture = System.Threading.Thread.CurrentThread.CurrentCulture;
        }

        public App()
        {
            InitializeComponent();

            ThemeViewModel = new ThemeViewModel();

#if DEBUG  // Never want this in release code
            ChangeCulture("es-US");
#endif

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
                    await App.Current.MainPage.DisplayAlert(AppResources.Alert,
                        String.Format(AppResources.NotificationReceivedFor, e.Title, e.Message, e.Id),
                        AppResources.OK);
                });

            };

            MessagingCenter.Subscribe<LeadersViewModel, string>(this, MessageConsts.RequestMeeting, async (sender, LeaderId) =>
            {
                await Task.Delay(TimeSpan.FromSeconds(2));

                var leader = await LeadersDataStore.GetItemAsync(LeaderId);

                await App.Current.MainPage.DisplayAlert(AppResources.MessageReceived,
                    string.Format(AppResources.MeetingSubmittedFor, leader.Name),
                    AppResources.OK);
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
