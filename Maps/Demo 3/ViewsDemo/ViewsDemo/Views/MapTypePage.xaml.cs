using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewsDemo.Services;
using ViewsDemo.ViewModels;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ViewsDemo.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MapTypePage : Popup
    {
        public MapTypePage()
        {
            InitializeComponent();

            MessagingCenter.Subscribe<MapViewModel>(this, MessageConsts.CloseMapTypePopup, (sender) =>
            {
                this.Dismiss(null);
            });
        }

        public MapTypePage(MapViewModel vm) : this()
        {
            BindingContext = vm;
        }
    }
}