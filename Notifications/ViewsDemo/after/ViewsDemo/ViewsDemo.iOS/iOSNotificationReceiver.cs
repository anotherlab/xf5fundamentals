using Foundation;
using System;
using UserNotifications;
using Xamarin.Forms;
using ViewsDemo.Interfaces;

namespace ViewsDemo.iOS
{
    public class iOSNotificationReceiver : UNUserNotificationCenterDelegate
    {
        // When an iOS apap is running in the foreground, this method will be called to the deliver the notification to the app
        public override void WillPresentNotification(UNUserNotificationCenter center, UNNotification notification, Action<UNNotificationPresentationOptions> completionHandler)
        {
            // Our code to process the notification
            ProcessNotification(notification);

            // Specify how to present the notification to the user
            completionHandler(UNNotificationPresentationOptions.Banner | UNNotificationPresentationOptions.List);
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

        void ProcessNotificationOld(UNNotification notification)
        {
            // Defined fields in UNNotificationContent that get mapped to string fields
            string title = notification.Request.Content.Title;
            string message = notification.Request.Content.Body;

            // The custom field comes over in a NSDictionary object 
            string id = "";

            // define the key name as NSString
            var idkey = new NSString("id");

            var foo = (notification.Request.Content.UserInfo.ObjectForKey(new NSString("id"))?.ToString() ?? "");
            var bar = (notification.Request.Content.UserInfo.ObjectForKey(new NSString("id1"))?.ToString() ?? "");


            // Check to see if the key exists in the dictionary
            if (notification.Request.Content.UserInfo.ContainsKey(idkey))
            {
                // if the key exists, get the value for the key and convert it to string
                id = notification.Request.Content.UserInfo.ValueForKey(idkey).ToString();
            }

            // Get a reference to the INotificationManager implementation and notify the shared code
            DependencyService.Get<INotificationManager>()?.ReceiveNotification(title, message, id);
        }
    }
}