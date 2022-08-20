using System;
using System.Collections.Generic;
using System.Text;

using Xamarin.Forms;
using Xamarin.Essentials;
using ViewsDemo.Interfaces;
using ViewsDemo.Models;

namespace ViewsDemo.ViewModels
{
    public class ThemeViewModel : BaseViewModel
    {
        const string keyName = "CurrentTheme";
        IStatusBarColor _StatusBarColor = null;
        IStatusBarColor StatusBarColor => _StatusBarColor ?? (_StatusBarColor = DependencyService.Get<IStatusBarColor>());

        OSAppTheme _CurrentTheme;
        public OSAppTheme CurrentTheme
        {
            get => _CurrentTheme;
            set
            {
                if (SetProperty(ref _CurrentTheme, value))
                {
                    Preferences.Set(keyName, (int)_CurrentTheme);
                    App.Current.UserAppTheme = _CurrentTheme;
                }
                UpdateStatusBarColor();
            }
        }

        public ThemeViewModel()
        {
            var NewTheme = OSAppTheme.Unspecified;

            if (Preferences.ContainsKey(keyName))
            {
                // Read the value from preferences, use Unspecified as the default
                NewTheme = (OSAppTheme)Preferences.Get(keyName, (int)OSAppTheme.Unspecified);

                // Safety check
                if (Enum.IsDefined(typeof(OSAppTheme), NewTheme))
                {
                    App.Current.UserAppTheme = NewTheme;
                }
            }

            CurrentTheme = NewTheme;
        }

        public void UpdateStatusBarColor()
        {
            try
            {
                if (Device.RuntimePlatform == Device.Android)
                {
                    var colorName = Application.Current.RequestedTheme == OSAppTheme.Dark ? "PrimaryDark" : "Primary";

                    if (Application.Current.Resources.TryGetValue(colorName, out var color))
                    {
                        if (color is Color)
                        {
                            StatusBarColor?.ChangeStatusBarColor(((Color)color).ToHex());
                        }
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
