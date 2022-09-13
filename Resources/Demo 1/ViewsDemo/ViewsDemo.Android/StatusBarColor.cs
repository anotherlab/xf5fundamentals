using Android.OS;
using System;
using ViewsDemo.Interfaces;
using Xamarin.Forms;

[assembly: Dependency(typeof(ViewsDemo.Droid.StatusBarColor))]
namespace ViewsDemo.Droid
{
    public class StatusBarColor : IStatusBarColor
    {
        public void ChangeStatusBarColor(string color)
        {
            try
            {
                if (Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop)
                {
                    Device.BeginInvokeOnMainThread(() =>
                        Xamarin.Essentials.Platform.CurrentActivity.Window.SetStatusBarColor(Android.Graphics.Color.ParseColor(color)));
                }
            }
            catch (Exception)
            {
            }
        }
    }
}