using Foundation;
using System;
using UserNotifications;
using Xamarin.Forms;
using ViewsDemo.Interfaces;

namespace ViewsDemo.iOS
{
    public class iOSNotificationReceiver : UNUserNotificationCenterDelegate
    {
        // When an iOS app is running in the background, this method will be called to the deliver the notification to the app
        public override void DidReceiveNotificationResponse(UNUserNotificationCenter center, UNNotificationResponse response, Action completionHandler)
        {
            // Our code to process the notification
            ProcessNotification(response.Notification);

            // Let the OS dispose of the notification
            completionHandler();
        }

        // When an iOS apap is running in the foreground, this method will be called to the deliver the notification to the app
        public override void WillPresentNotification(UNUserNotificationCenter center, UNNotification notification, Action<UNNotificationPresentationOptions> completionHandler)
        {
            // Our code to process the notification
            ProcessNotification(notification);

            // Specify how to present the notification to the user
            // Since we are diosplaying an alert message, we don't need to display a notification
            completionHandler(UNNotificationPresentationOptions.None);
            // if you do want to display a notification, then use this line
            //completionHandler(UNNotificationPresentationOptions.Banner | UNNotificationPresentationOptions.List);
        }

        void ProcessNotification(UNNotification notification)
        {
            // Defined fields in UNNotificationContent that get mapped to string fields
            string title = notification.Request.Content.Title;
            string message = notification.Request.Content.Body;

            // The custom field comes over in a NSDictionary object 
            // We use the null-coalescing operator to make sure we have a value before calling ToString() on it
            var id = (notification.Request.Content.UserInfo.ObjectForKey(new NSString("id"))?.ToString() ?? "");

            // Get a reference to the INotificationManager implementation and notify the shared code
            DependencyService.Get<INotificationManager>()?.ReceiveNotification(title, message, id);
        }

    }
}