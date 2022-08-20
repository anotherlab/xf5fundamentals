using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewsDemo.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ViewsDemo.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    [QueryProperty("ID", "id")]
    public partial class LeaderPage : ContentPage
    {
        public string ID
        {
            set
            {
                var DataStore = (Application.Current as App).LeadersDataStore;

                var leader = DataStore.GetItemAsync(Uri.UnescapeDataString(value)).Result;

                BindingContext = new LeaderViewModel(leader);
            }
        }
        public LeaderPage()
        {
            InitializeComponent();
        }
    }
}