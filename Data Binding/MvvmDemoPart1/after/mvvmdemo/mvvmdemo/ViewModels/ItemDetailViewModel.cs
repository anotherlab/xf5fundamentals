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

        public async void LoadItemId(string itemId)
        {
            try
            {
                item = await DataStore.GetItemAsync(itemId);
                Text = item.Text;
                Description = item.Description;
            }
            catch (Exception)
            {
                Debug.WriteLine("Failed to Load Item");
            }
        }


        public ItemDetailViewModel()
        {
            SaveCommand = new Command(async () => await ExecuteSave(), CanExecuteSave);
            this.PropertyChanged += (_, __) => SaveCommand.ChangeCanExecute();
        }

        private bool CanExecuteSave()
        {
            if (item != null)
                return item.Description != Description || item.Text != Text;
            return false;
        }

        private async Task ExecuteSave()
        {
            item.Description = Description;
            item.Text = Text;

            //await Shell.Current.Navigation.PopAsync();
            await Shell.Current.GoToAsync("..");
        }

    }
}
