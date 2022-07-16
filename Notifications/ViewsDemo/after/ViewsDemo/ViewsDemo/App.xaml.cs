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

        INotificationManager notificationManager;

        public App()
        {
            InitializeComponent();
            MainPage = new AppShell();

            // demo 1

            notificationManager = DependencyService.Get<INotificationManager>();
            notificationManager.NotificationReceived += (sender, eventArgs) =>
            {
                var e = (NotificationEventArgs)eventArgs;
                //ShowNotification(evtData.Title, evtData.Message, evtData.Id);

                Device.BeginInvokeOnMainThread(async () =>
                {
                    await App.Current.MainPage.DisplayAlert("Alert", $"Notification Received:\nTitle: {e.Title}\nMessage: {e.Message}\nID: {e.Id}", "Ok");
                });

            };

            // demo 2

            MessagingCenter.Subscribe<LeadersViewModel, string>(this, MessageConsts.RequestMeeting, async (sender, LeaderId) =>
            {
                await Task.Delay(TimeSpan.FromSeconds(2));

                var leader = await LeadersDataStore.GetItemAsync(LeaderId);

                await App.Current.MainPage.DisplayAlert("Message received", $"Meeting request submitted for {leader.Name}", "OK");
            });
        }

        // demo 1
        public void SendNotification(string title, string message, string id)
        {
            notificationManager.SendNotification(title, message, id);
        }

        void ShowNotification(string title, string message, string id)
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                await App.Current.MainPage.DisplayAlert("Alert", $"Notification Received:\nTitle: {title}\nMessage: {message}\nID: {id}", "Ok");
            });
        }

        protected override void OnStart() { }

        protected override void OnSleep() { }

        protected override void OnResume() { }
    }
}
