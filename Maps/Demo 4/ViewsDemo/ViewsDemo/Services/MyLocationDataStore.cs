using System.Collections.Generic;
using ViewsDemo.Models;

namespace ViewsDemo.Services
{
    public class MyLocationDataStore
    {
        public List<MyLocation> locations { get; private set; }

        public MyLocationDataStore()
        {
            locations = GetLocations();
        }

        private List<MyLocation> GetLocations()
        {
            List<MyLocation> locations = new List<MyLocation>();

            locations.Add(new MyLocation { Name = "Homewood Suites", Latitude = 40.504487382208254, Longitude = -111.90460525267225 });
            locations.Add(new MyLocation { Name = "Hampton Inn", Latitude = 40.5037694985628, Longitude = -111.8963655075429 });
            locations.Add(new MyLocation { Name = "TownePlace Suites", Latitude = 40.5041610724128, Longitude = -111.88941322259002 });

            return locations;
        }
    }
}
