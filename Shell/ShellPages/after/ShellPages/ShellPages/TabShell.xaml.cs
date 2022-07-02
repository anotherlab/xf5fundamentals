using ShellPages.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ShellPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TabShell : Xamarin.Forms.Shell
    {
        public TabShell()
        {
            InitializeComponent();
        }
        protected override void OnNavigating(ShellNavigatingEventArgs args)
        {
            base.OnNavigating(args);
            var route = args.Target.Location.OriginalString.ToLower();
            vmProperty.Parameter = route;
        }
    }
}