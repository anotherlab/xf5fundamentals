using System;
using System.Collections.Generic;
using System.Text;

namespace ViewsDemo.Interfaces
{
    public interface INotificationManager
    {
        event EventHandler NotificationReceived;
        void Initialize();
        void SendNotification(string title, string message, string id);
        void ReceiveNotification(string title, string message, string id);
    }

    public class NotificationEventArgs : EventArgs
    {
        public string Title { get; set; }
        public string Message { get; set; }
        public string Id { get; set; }
    }
}
