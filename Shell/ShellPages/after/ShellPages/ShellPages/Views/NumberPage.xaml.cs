using ShellPages.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ShellPages.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NumberPage : ContentPage
    {
        public NumberPage()
        {
            InitializeComponent();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            (BindingContext as SampleViewModel)?.OnAppearing();
        }
    }
}