using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ViewsDemo.Models;
using ViewsDemo.Views;
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

        public Command<Leader> LeaderTapped => new Command<Leader>(async (leader) =>
        {
            if (leader == null)
                return;

            await Shell.Current.GoToAsync($"leader?id={leader.Id}");
        });

        // demo 1
        public Command<Leader> NotifyLeader => new Command<Leader>(async (leader) =>
        {
            if (leader == null)
                return;

            await Task.Run(() => (Application.Current as App).SendNotification("Leader Notified", leader.Name, leader.Id));
        });

        // demo 2
        public Command<Leader> RequestMeeting => new Command<Leader>((leader) =>
        {
            if (leader == null)
                return;

            MessagingCenter.Send<LeadersViewModel, string>(this, MessageConsts.RequestMeeting, leader.Id);

            //MessagingCenter.Send(this, MessageConsts.RequestMeeting, leader.Id);
        });


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
