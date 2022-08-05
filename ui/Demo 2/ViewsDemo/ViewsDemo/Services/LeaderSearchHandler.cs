using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using ViewsDemo.Models;
using System.Threading.Tasks;

namespace ViewsDemo.Services
{
    public class LeaderSearchHandler : SearchHandler
    {
        protected override void OnQueryChanged(string oldValue, string newValue)
        {
            base.OnQueryChanged(oldValue, newValue);

            if (string.IsNullOrWhiteSpace(newValue))
            {
                ItemsSource = null;
            }
            else
            {
                var DataStore = (Application.Current as App).LeadersDataStore;

                // Find the all of the leaders with a name that contains the search text
                ItemsSource = DataStore.GetAll()
                    .Where(j => j.Name.ToLower().Contains(newValue.ToLower()))
                    .ToList();
            }
        }

        protected override async void OnItemSelected(object item)
        {
            base.OnItemSelected(item);

            var uri = $"leader?id={((Leader)item).Id}";

            // work around for bug 5713
            // https://github.com/xamarin/Xamarin.Forms/issues/5713
            await Task.Delay(400);

            await Shell.Current.GoToAsync(uri, true);

        }
    }
}
