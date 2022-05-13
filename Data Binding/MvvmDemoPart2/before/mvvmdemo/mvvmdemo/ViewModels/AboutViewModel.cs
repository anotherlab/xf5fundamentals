using System;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace mvvmdemo.ViewModels
{
    public class AboutViewModel : BaseViewModel
    {
        public string Platform { get; } = DeviceInfo.Platform.ToString();
        public string Version { get; } = DeviceInfo.Version.ToString();
        public string DeviceName { get; } = DeviceInfo.Name;
        public string DeviceType { get; } = DeviceInfo.DeviceType.ToString();

        public AboutViewModel()
        {
            Title = "About";
        }
    }
}