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
        LoginPage page;
        public MainPage()
        {
            InitializeComponent();
        }

        public async void PushPage(object sender, EventArgs e)
        {
            var flexPage = new MyNewPage { Level = 1 };
            await Navigation.PushAsync(flexPage);
        }
        public async void PushPageNoAnimation(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MyNewPage { Level = 1 }, false);
        }

        public async void ModalButton(object sender, EventArgs e)
        {
            App.Current.ModalPopping += Current_ModalPopping;
            page = new LoginPage();
            await Navigation.PushModalAsync(page);
        }

        private void Current_ModalPopping(object sender, ModalPoppingEventArgs e)
        {
            if (e.Modal == page)
            {
                e.Cancel = !page.loginSuccess;

                if (page.loginSuccess)
                    App.Current.ModalPopping -= Current_ModalPopping;
                else
                    page.DisplayErrorMessage = "Login failed";
            }
        }
    }
}
