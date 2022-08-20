﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using ViewsDemo.Models;
using ViewsDemo.Services;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

// Eliminate any confusion with Xamarin.Essentials.Maps
using Maps = Xamarin.Forms.Maps;

namespace ViewsDemo.ViewModels
{
    public class MapViewModel : BaseViewModel
    {
        private Maps.Map _Map;
        public Maps.Map Map
        {
            get => _Map;
            set
            {
                SetProperty(ref _Map, value);
                MapReset();
            }
        }

        readonly MyLocation office;

        public Command StreetCommand { get; }
        public Command SatelliteCommand { get; }
        public Command HybridCommand { get; }
        public Command GetHotelsCommand { get; }

        private void AddLocalHotels()
        {
            MapReset();

            var extent = new MyExtent(office.Latitude, office.Longitude);

            var DataStore = (Application.Current as App).MyLocationDataStore;

            foreach (var hotel in DataStore.locations)
            {
                extent.ReadLatLon(hotel.Latitude, hotel.Longitude, false);
                Map.Pins.Add(PinFromLocation(hotel));
            }

            var pin = PinFromLocation(office);
            pin.Type = PinType.SavedPin;
            Map.Pins.Add(pin);

            var center = extent.GetCenter();
            var radius = extent.GetDistance() * 0.6;

            var mapspan = MapSpan.FromCenterAndRadius(center, Maps.Distance.FromKilometers(radius));

            Map.MoveToRegion(mapspan);
        }

        private void SetMapType(MapType mapType)
        {
            // Update the map
            Map.MapType = mapType;

            // Tell the pop up to close
            MessagingCenter.Send<MapViewModel>(this, MessageConsts.CloseMapTypePopup);
        }

        public MapViewModel()
        {
            office = new MyLocation
            {
                Name = "Office",
                Latitude = 40.48628643849999,
                Longitude = -111.89013511002912
            };

            StreetCommand = new Command(() => { SetMapType(MapType.Street); });
            SatelliteCommand = new Command(() => { SetMapType(MapType.Satellite); });
            HybridCommand = new Command(() => { SetMapType(MapType.Hybrid); });
            GetHotelsCommand = new Command(AddLocalHotels);

        }

        public void CenterToHeadQuarters()
        {
            var position = new Position(office.Latitude, office.Longitude);

            var mapspan = MapSpan.FromCenterAndRadius(position, Maps.Distance.FromKilometers(1.0));

            Map.MoveToRegion(mapspan);
        }

        private void MapReset()
        {
            Map.MapElements.Clear();
            Map.Pins.Clear();
            Map.Pins.Add(PinFromLocation(office, PinType.SavedPin));
        }

        private Pin PinFromLocation(MyLocation myLocation, PinType pinType = PinType.Place)
        {
            var pin = new Pin
            {
                Label = myLocation.Name,
                Type = pinType,
                Position = new Position(myLocation.Latitude, myLocation.Longitude)
            };

            return pin;
        }
    }
}