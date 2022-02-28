using MyFirstApp.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;

namespace MyFirstApp.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel();
        }
    }
}