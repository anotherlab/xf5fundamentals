using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ViewsDemo.Models;
using Xamarin.Forms;

namespace ViewsDemo.ViewModels
{
    public class LeadersViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Leader> Leaders { get; private set; } = new ObservableCollection<Leader>();

        bool isRefreshing;

        public bool IsRefreshing
        {
            get { return isRefreshing; }
            set
            {
                isRefreshing = value;
                OnPropertyChanged();
            }
        }


        public ICommand DeleteCommand => new Command<Leader>(RemoveLeader);

        void RemoveLeader(Leader leader)
        {
            if (Leaders.Contains(leader))
                Leaders.Remove(leader);
        }


        public ICommand RefreshCommand => new Command(async () => await RefreshItemsAsync());

        async Task RefreshItemsAsync()
        {
            IsRefreshing = true;
            Leaders.Clear();
            await Task.Delay(TimeSpan.FromSeconds(2)); 
            AddLeaders();
            IsRefreshing = false;
        }

        public LeadersViewModel()
        {
            AddLeaders();
        }

        void AddLeaders()
        {
            var DataStore = (Application.Current as App).LeadersDataStore;

            Leaders = new ObservableCollection<Leader>(DataStore.GetAll());
        }

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
