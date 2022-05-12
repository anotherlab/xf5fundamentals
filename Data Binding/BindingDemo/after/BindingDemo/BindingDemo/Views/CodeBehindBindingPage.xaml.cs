using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BindingDemo.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CodeBehindBindingPage : ContentPage
    {
        public CodeBehindBindingPage()
        {
            InitializeComponent();
            MyLabel.BindingContext = MySlider;

            // Bind to another control
            MyLabel.SetBinding(Label.RotationProperty, "Value");
            MyLabel.SetBinding(Label.TextProperty, "Value", BindingMode.Default, null, "{0:F1}°");

            // Bind to variable
            var today = DateTime.Now;
            DayName.SetBinding(Label.TextProperty, new Binding("DayOfWeek", source: today));
        }
    }
}