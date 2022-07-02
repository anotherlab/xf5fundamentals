using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace ShellPages.Views
{
    public class ShellSwitchPage : ContentPage
    {
        public ShellSwitchPage()
        {
            Content = new StackLayout
            {
                Children = {
                    new Label { Text = "Welcome to Xamarin.Forms!" }
                }
            };
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();

            Application.Current.MainPage = (App.Current as App).FlyoutShell;
        }
    }
}