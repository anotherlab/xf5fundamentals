using CoreFoundation;
using Foundation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UIKit;
using ViewsDemo.Interfaces;
using Xamarin.Forms;

namespace ViewsDemo.iOS
{
    public class ToastMessage : IToastMessage
    {
        public void OpenToast(string text)
        {
            var rvc = UIApplication.SharedApplication.KeyWindow.RootViewController;

            var alertController = UIAlertController.Create(string.Empty,
                text,
                UIAlertControllerStyle.Alert);

            alertController.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Default, null));

            rvc.PresentViewController(alertController, true, () =>
            {
                var delayTime = new DispatchTime(DispatchTime.Now, 3000000000);
                DispatchQueue.MainQueue.DispatchAfter(delayTime, () =>
                {
                    alertController.DismissViewController(true, null);
                });
            });
        }
    }
}