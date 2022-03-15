using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AppEvents
{
    public partial class MainPage : ContentPage
    {
        Page childPage;

        public MainPage()
        {
            InitializeComponent();
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            childPage = new ChildPage();
            App.Current.ModalPopping += MainPageModalPopping;
            Navigation.PushModalAsync(childPage);
        }

        private void MainPageModalPopping(object sender, ModalPoppingEventArgs e)
        {
            if (e.Modal == childPage)
            {
                Debug.WriteLine("ParentPage: " + sender.GetType().Name, "PS");
                App.Current.ModalPopping -= MainPageModalPopping;
            }
        }
    }
}
