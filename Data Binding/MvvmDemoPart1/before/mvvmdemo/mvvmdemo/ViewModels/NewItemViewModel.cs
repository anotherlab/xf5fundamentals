using mvvmdemo.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace mvvmdemo.ViewModels
{
    public class NewItemViewModel : BaseViewModel
    {
        private string text;
        public string Text
        {
            get => text;
            set => SetProperty(ref text, value);
        }

        private string description;
        public string Description
        {
            get => description;
            set => SetProperty(ref description, value);
        }

        public Command SaveCommand { get; }
        public Command CancelCommand { get; }

        public NewItemViewModel()
        {
        }
    }
}
