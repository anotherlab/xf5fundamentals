using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;

namespace ShellPages.ViewModels
{
    public class ShellViewModel : BaseViewModel
    {
        public string ImageUrl => "XF Leaders.png";
        public string Version => VersionTracking.CurrentVersion;
    }
}
