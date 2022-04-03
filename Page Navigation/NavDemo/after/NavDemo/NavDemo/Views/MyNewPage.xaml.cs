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
    public partial class MyNewPage : ContentPage
    {
        int _level = 0;
        public int Level
        { 
            get => _level; 
            set
            {
                if (_level != value)
                {
                    _level = value;
                    MyLabel.Text = $"Welcome to level {Level}";
                }
            } 
        }
        public MyNewPage()
        {
            InitializeComponent();
        }

        private void PopClick(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }

        private void PushClick(object sender, EventArgs e)
        {
            var page = new MyNewPage();
            page.Level = Level + 1;
            Navigation.PushAsync(page);
        }
    }
}