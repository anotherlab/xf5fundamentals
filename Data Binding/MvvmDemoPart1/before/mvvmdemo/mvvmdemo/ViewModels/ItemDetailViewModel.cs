using mvvmdemo.Models;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace mvvmdemo.ViewModels
{
    [QueryProperty(nameof(ItemId), nameof(ItemId))]
    public class ItemDetailViewModel : BaseViewModel
    {
        private Item item;
        public Command SaveCommand { get; private set; }

        private string text;
        public string Text
        {
            get => text;
            set
            {
                if (SetProperty(ref text, value))
                {
                    if (value != item.Text)
                        Title = $"Modified {item.Text}";
                    else
                        Title = $"{item.Text}";
                }
            }
        }

        private string description;
        public string Description
        {
            get => description;
            set => SetProperty(ref description, value);
        }

        private string itemId;
        public string ItemId
        {
            get => itemId;
            set
            {
                itemId = value;
                LoadItemId(value);
            }
        }

        void LoadItemId(object value) { }

        public ItemDetailViewModel()
        {
        }

    }
}
