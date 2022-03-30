using NavDemo.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace NavDemo
{
    public partial class MainPage : ContentPage
    {
        FramePage page;
        public MainPage()
        {
            InitializeComponent();
        }

        public async void PushFlex(object sender, EventArgs e)
        {
            var flexPage = new FlexPage();
            await Navigation.PushAsync(flexPage);
        }
        public async void PushFlexNoAnimation(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new FlexPage(), false);
        }

        public async void ModalButton(object sender, EventArgs e)
        {
            App.Current.ModalPopping += Current_ModalPopping;
            page = new FramePage();
            await Navigation.PushModalAsync(page);
        }

        private void Current_ModalPopping(object sender, ModalPoppingEventArgs e)
        {
            if (e.Modal == page)
            {
                e.Cancel = !page.loginSuccess;

                if (page.loginSuccess)
                    App.Current.ModalPopping -= Current_ModalPopping;
            }
        }

    }
}
