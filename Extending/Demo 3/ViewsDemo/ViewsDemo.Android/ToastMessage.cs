using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ViewsDemo.Interfaces;
using Xamarin.Forms;

namespace ViewsDemo.Droid
{
    public class ToastMessage : IToastMessage
    {
        public void OpenToast(string text)
        {
            Toast.MakeText(Android.App.Application.Context, text, ToastLength.Long).Show();
        }
    }
}