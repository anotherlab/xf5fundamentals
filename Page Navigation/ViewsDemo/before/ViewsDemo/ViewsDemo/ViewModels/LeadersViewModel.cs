using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using ViewsDemo.Models;

namespace ViewsDemo.ViewModels
{
    public class LeadersViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Leader> Leaders { get; private set; } = new ObservableCollection<Leader>();

        public LeadersViewModel()
        {
            AddLeaders();
        }

        void AddLeaders()
        {
            Leaders.Add(new Leader
            {
                Name = "John Smyth",
                JobTitle = "Director of Fun",
                ImageUrl = "thum000.jpg"
            });
            Leaders.Add(new Leader
            {
                Name = "Jane Linsey",
                JobTitle = "HR Manager",
                ImageUrl = "thum001.jpg"
            });
            Leaders.Add(new Leader
            {
                Name = "Rob Chaney",
                JobTitle = "Dynamic Assurance Associate",
                ImageUrl = "thum002.jpg"
            });

            Leaders.Add(new Leader
            {
                Name = "Esther Reeves",
                JobTitle = "Future Team Assistant",
                ImageUrl = "thum003.jpg"
            });
            Leaders.Add(new Leader
            {
                Name = "Victor Conner",
                JobTitle = "Director of Deep Thinking",
                ImageUrl = "thum004.jpg"
            });
            Leaders.Add(new Leader
            {
                Name = "Liz Torres",
                JobTitle = "Senior Assurance Guru",
                ImageUrl = "thum005.jpg"
            });
            Leaders.Add(new Leader
            {
                Name = "Maria Ross",
                JobTitle = "Legacy Program Administrator",
                ImageUrl = "thum006.jpg"
            });
            Leaders.Add(new Leader
            {
                Name = "David Guo",
                JobTitle = "Corporate Accounts Manager",
                ImageUrl = "thum007.jpg"
            });
            Leaders.Add(new Leader
            {
                Name = "Sabrina Gomez-Gardner",
                JobTitle = "Customer Happiness Director",
                ImageUrl = "thum008.jpg"
            });
            Leaders.Add(new Leader
            {
                Name = "Robert Marcos",
                JobTitle = "Chief Tactics Associate",
                ImageUrl = "thum009.jpg"
            });
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
