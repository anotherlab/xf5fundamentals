using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NavDemo.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FlexPage : ContentPage
    {
        Random random = new Random();
        public FlexPage()
        {
            InitializeComponent();

            MakeSomeBoxes();
        }
        private void MakeSomeBoxes()
        {
            for (int i = 0; i < 40; i++)
            {
                var boxview = new BoxView
                {
                    Color = Color.FromRgb(random.Next(256), random.Next(256), random.Next(256)),
                    WidthRequest = 60 + random.Next(60),
                    HeightRequest = 60 + random.Next(10)
                };
                flexLayout.Children.Add(boxview);
            }
        }
    }
}