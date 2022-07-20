using System;
using Foundation;
using UserNotifications;
using Xamarin.Forms;
using ViewsDemo.Interfaces;

[assembly: Dependency(typeof(ViewsDemo.iOS.iOSNotificationManager))]
namespace ViewsDemo.iOS
{
    public class iOSNotificationManager : INotificationManager
    {
        int messageId = 0;
        bool hasNotificationsPermission;
        public event EventHandler NotificationReceived;

        public iOSNotificationManager() => Initialize();

        public void Initialize()
        {
            // request the permission to use local notifications
            UNUserNotificationCenter.Current.RequestAuthorization(UNAuthorizationOptions.Alert | UNAuthorizationOptions.Sound, (approved, err) =>
            {
                hasNotificationsPermission = approved;
            });
        }

        public void SendNotification(string title, string message, string id)
        {
            // If permissions were not requested or denied by the user, then it's an early exit
            if (!hasNotificationsPermission)
            {
                return;
            }

            messageId++;

            var content = new UNMutableNotificationContent()
            {
                Title = title,
                Subtitle = "",
                Body = message,
                UserInfo = new NSDictionary(new NSString("id"), new NSString(id)),
                Badge = 1
            };


            // Create a time-based trigger, interval is in seconds and must be greater than 0.
            var trigger = UNTimeIntervalNotificationTrigger.CreateTrigger(5.25, false);

            var request = UNNotificationRequest.FromIdentifier(messageId.ToString(), content, trigger);

            UNUserNotificationCenter.Current.AddNotificationRequest(request, (err) =>
            {
                if (err != null)
                {
                    throw new Exception($"Failed to schedule notification: {err}");
                }
            });
        }

        public void ReceiveNotification(string title, string message, string id)
        {
            var args = new NotificationEventArgs()
            {
                Title = title,
                Message = message,
                Id = id
            };

            NotificationReceived?.Invoke(null, args);
        }
    }
}