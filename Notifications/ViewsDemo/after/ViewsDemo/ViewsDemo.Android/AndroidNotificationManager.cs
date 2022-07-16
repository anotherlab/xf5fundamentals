using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using AndroidX.Core.App;
using System;
using ViewsDemo.Interfaces;
using Xamarin.Forms;
using AndroidApp = Android.App.Application;

[assembly: Dependency(typeof(ViewsDemo.Droid.AndroidNotificationManager))]
namespace ViewsDemo.Droid
{
    public class AndroidNotificationManager : INotificationManager
    {
        // Notification properties
        const string channelId = "default";
        const string channelName = "Default";
        const string channelDescription = "The default channel for notifications.";

        public const string TitleKey = "title";
        public const string MessageKey = "message";
        public const string IdKey = "leaderid";

        bool channelInitialized = false;
        int messageId = 0;
        int pendingIntentId = 0;

        NotificationManager manager;

        public event EventHandler NotificationReceived;

        public static AndroidNotificationManager Instance { get; private set; }

        public AndroidNotificationManager() => Initialize();

        public void Initialize()
        {
            if (Instance == null)
            {
                CreateNotificationChannel();
                Instance = this;
            }
        }

        // The intent will call ReceiveNotification, which call the NotificationReceived event
        // defined in app.xaml.cs
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


        // The SendNotifification method in app.xaml.cs will call SendNotification
        public void SendNotification(string title, string message, string id)
        {
            if (!channelInitialized)
            {
                CreateNotificationChannel();
            }

            // Code that does something in the background
            // themn fires off a message

            Show(title, message, id);
        }

        public void Show(string title, string message, string id)
        {
            Intent intent = new Intent(AndroidApp.Context, typeof(MainActivity));
            intent.PutExtra(TitleKey, title);
            intent.PutExtra(MessageKey, message);
            intent.PutExtra(IdKey, id);

            PendingIntent pendingIntent = PendingIntent.GetActivity(AndroidApp.Context, pendingIntentId++, intent, PendingIntentFlags.UpdateCurrent);

            NotificationCompat.Builder builder = new NotificationCompat.Builder(AndroidApp.Context, channelId)
                .SetContentIntent(pendingIntent)
                .SetContentTitle(title)
                .SetContentText(message)
                .SetLargeIcon(BitmapFactory.DecodeResource(AndroidApp.Context.Resources, Resource.Drawable.xflogo))
                .SetSmallIcon(Resource.Drawable.xflogo)
                .SetDefaults((int)NotificationDefaults.Sound | (int)NotificationDefaults.Vibrate)
                .SetAutoCancel(true);

            Notification notification = builder.Build();

            manager.Notify(messageId++, notification);
        }

        void CreateNotificationChannel()
        {
            manager = (NotificationManager)AndroidApp.Context.GetSystemService(AndroidApp.NotificationService);

            if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
            {
                var channelNameJava = new Java.Lang.String(channelName);
                var channel = new NotificationChannel(channelId, channelNameJava, NotificationImportance.Default)
                {
                    Description = channelDescription
                };
                manager.CreateNotificationChannel(channel);
            }

            channelInitialized = true;
        }
    }
}