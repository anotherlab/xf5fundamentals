using System;
using System.Collections.Generic;
using System.Text;

namespace ShellPages.ViewModels
{
    public static class vmProperty
    {
        public static string Parameter;
    }

    public class SampleViewModel : BaseViewModel
    {
        string parameter = string.Empty;
        public string Parameter
        {
            get { return parameter; }
            set { SetProperty(ref parameter, value); }
        }

        public void OnAppearing()
        {
            Parameter = vmProperty.Parameter;
            Title = $"NumberPage {Parameter}";
        }
    }
}
