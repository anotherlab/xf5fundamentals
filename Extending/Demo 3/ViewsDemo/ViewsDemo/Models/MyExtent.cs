using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms.Maps;

namespace ViewsDemo.Models
{
    public class MyExtent
    {
        public double MinLatitude { get; set; }
        public double MinLongitude { get; set; }
        public double MaxLatitude { get; set; }
        public double MaxLongitude { get; set; }

        public void ReadLatLon(Position position, bool isFirstPoint)
        {
            ReadLatLon(position.Latitude, position.Longitude, isFirstPoint);
        }

        public void ReadLatLon(double lat, double lon, bool isFirstPoint)
        {
            if (isFirstPoint)
            {
                MinLongitude = lon;
                MaxLongitude = lon;
                MinLatitude = lat;
                MaxLatitude = lat;
            }
            else
            {
                if (lon < MinLongitude)
                    MinLongitude = lon;
                if (lat < MinLatitude)
                    MinLatitude = lat;
                if (lon > MaxLongitude)
                    MaxLongitude = lon;
                if (lat > MaxLatitude)
                    MaxLatitude = lat;
            }
        }
        public MyExtent() { }

        public MyExtent(double latitude, double longitude)
        {
            ReadLatLon(latitude, longitude, true);
        }
        public MyExtent(double latitude, double longitude, double otherLatitude, double otherLongitude) : this(latitude, longitude)
        {
            ReadLatLon(otherLatitude, otherLongitude, false);
        }

        // Simple way of getting center, reasonably accurate for short distances
        public Position GetCenter()
        {
            var Latitude = (MaxLatitude + MinLatitude) / 2.0;
            var Longitude = (MaxLongitude + MinLongitude) / 2.0;

            return new Position(Latitude, Longitude);
        }
        public double GetDistance()
        {
            // https://docs.microsoft.com/en-us/xamarin/essentials/geolocation?tabs=android#calculate-distance
            Location start = new Location(MinLatitude, MinLongitude);
            Location end = new Location(MaxLatitude, MaxLongitude);

            return Location.CalculateDistance(start, end, DistanceUnits.Kilometers);
        }
    }
}
