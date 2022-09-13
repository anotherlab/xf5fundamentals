using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using ViewsDemo.Controls;
using ViewsDemo.Models;
using ViewsDemo.Resources;
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
        private PsMap _Map;
        public PsMap Map
        {
            get => _Map;
            set
            {
                SetProperty(ref _Map, value);
                MapReset();
            }
        }

        private MyLocation _SelectedHotel = null;

        public MyLocation SelectedHotel
        {
            get => _SelectedHotel;
            set { SetProperty(ref _SelectedHotel, value); }
        }

        readonly MyLocation office;

        public Command StreetCommand { get; }
        public Command SatelliteCommand { get; }
        public Command HybridCommand { get; }
        public Command GetHotelsCommand { get; }
        public Command GetDirectionsCommand { get; }
        public Command SearchCommand { get; }
        public Command HereCommand { get; }

        private void CenterToLatLon(string LocationName, double Latitude, double Longitude)
        {
            var locatedAddress = new MyLocation
            {
                Name = LocationName,
                Latitude = Latitude,
                Longitude = Longitude
            };

            var position = new Position(locatedAddress.Latitude, locatedAddress.Longitude);

            var mapspan = MapSpan.FromCenterAndRadius(position, Maps.Distance.FromKilometers(1.0));

            Map.MoveToRegion(mapspan);
        }

        public async void DoSearch(string address)
        {
            try
            {
                var locations = await Geocoding.GetLocationsAsync(address);

                var location = locations?.FirstOrDefault();

                if (location != null)
                {
                    var position = new Position(location.Latitude, location.Longitude);

                    MapReset();

                    var locatedAddress = new MyLocation
                    {
                        Name = address,
                        Latitude = location.Latitude,
                        Longitude = location.Longitude
                    };

                    AddPinFromLocation(locatedAddress, PsPinType.Here);
                    CenterToLatLon(address, location.Latitude, location.Longitude);
                }
            }
            catch (Exception ex)
            {

                Console.WriteLine($"Geocoding error: {ex.Message}");
            }
        }

        public async Task<string> ReverseGeoCoding(double Latitude, double Longitude)
        {
            string geocodeAddress = String.Empty;

            try
            {
                var placemarks = await Geocoding.GetPlacemarksAsync(Latitude, Longitude);

                var placemark = placemarks?.FirstOrDefault();

                if (placemark != null)
                {
                    geocodeAddress =
                        $"Latitude:        {Latitude}\n" +
                        $"Longitude:       {Longitude}\n" +
                        $"AdminArea:       {placemark.AdminArea}\n" +
                        $"CountryCode:     {placemark.CountryCode}\n" +
                        $"CountryName:     {placemark.CountryName}\n" +
                        $"FeatureName:     {placemark.FeatureName}\n" +
                        $"Locality:        {placemark.Locality}\n" +
                        $"PostalCode:      {placemark.PostalCode}\n" +
                        $"SubAdminArea:    {placemark.SubAdminArea}\n" +
                        $"SubLocality:     {placemark.SubLocality}\n" +
                        $"SubThoroughfare: {placemark.SubThoroughfare}\n" +
                        $"Thoroughfare:    {placemark.Thoroughfare}\n";
                }
            }
            catch (Exception ex)
            {
                geocodeAddress = $"Error: {ex.Message}";
            }

            return geocodeAddress;
        }

        public async void DoCurrentLocation()
        {
            try
            {
                var location = await Geolocation.GetLastKnownLocationAsync();

                if (location != null)
                {
                    MapReset();
                    var locatedAddress = new MyLocation
                    {
                        Name = AppResources.Here,
                        Latitude = location.Latitude,
                        Longitude = location.Longitude
                    };

                    AddPinFromLocation(locatedAddress, PsPinType.Here);

                    CenterToLatLon(AppResources.Here, location.Latitude, location.Longitude);
                }
                else
                {
                    await GetCurrentLocation();
                }
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                // Handle not supported on device exception
            }
            catch (FeatureNotEnabledException fneEx)
            {
                // Handle not enabled on device exception
            }
            catch (PermissionException pEx)
            {
                // Handle permission exception
            }
            catch (Exception ex)
            {
                // Unable to get location
            }
        }

        CancellationTokenSource cts;
        async Task GetCurrentLocation()
        {
            try
            {
                var request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10));
                cts = new CancellationTokenSource();
                var location = await Geolocation.GetLocationAsync(request, cts.Token);

                if (location != null)
                {
                    MapReset();

                    CenterToLatLon("Here", location.Latitude, location.Longitude);
                }
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                // Handle not supported on device exception
            }
            catch (FeatureNotEnabledException fneEx)
            {
                // Handle not enabled on device exception
            }
            catch (PermissionException pEx)
            {
                // Handle permission exception
            }
            catch (Exception ex)
            {
                // Unable to get location
            }
        }

        private bool CanDoDirections()
        {
            return SelectedHotel != null;
        }

        private void PinClicked(object sender, PinClickedEventArgs e)
        {
            try
            {
                var pin = sender as Pin;

                if (pin.Type == PinType.Place)
                {
                    var DataStore = (Application.Current as App).MyLocationDataStore;
                    SelectedHotel = DataStore.locations.FirstOrDefault(s => s.Name == pin.Label);
                }
            }
            catch (Exception)
            {
            }
        }

        private void AddLocalHotels()
        {
            MapReset();

            var extent = new MyExtent(office.Latitude, office.Longitude);

            var DataStore = (Application.Current as App).MyLocationDataStore;

            foreach (var hotel in DataStore.locations)
            {
                extent.ReadLatLon(hotel.Latitude, hotel.Longitude, false);

                AddPinFromLocation(hotel, PsPinType.Hotel);
            }

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
            GetDirectionsCommand = new Command(DoDirections, CanDoDirections);
            SearchCommand = new Command<string>((o) => DoSearch(o));
            HereCommand = new Command(DoCurrentLocation);

            this.PropertyChanged +=
                (_, __) => GetDirectionsCommand.ChangeCanExecute();
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
            Map.PsPins.Clear();
            Map.Pins.Clear();
            AddPinFromLocation(office, PsPinType.Office);
        }

        private PsPin PinFromLocation(MyLocation myLocation, PsPinType pinType = PsPinType.Office)
        {
            var pin = new PsPin
            {
                Label = myLocation.Name,
                Type = PinType.Place,
                PinType = pinType,
                Position = new Position(myLocation.Latitude, myLocation.Longitude)
            };

            pin.MarkerClicked += PinClicked;

            return pin;
        }

        private void AddPinFromLocation(MyLocation myLocation, PsPinType pinType = PsPinType.Office)
        {
            Map.AddPin(PinFromLocation(myLocation, pinType));
        }

        private async void DoDirections()
        {
            var gds = new GoogleDirectionsService
            {
                ApiKey = APIKeys.API_KEY
            };

            var directions = await gds.GetDirections(SelectedHotel.Latitude, SelectedHotel.Longitude,
                                            office.Latitude, office.Longitude);

            if (directions != null)
            {
                if ((directions.routes != null) && (directions.routes.Count() > 0))
                {
                    var route = directions.routes[0];
                    var points = GooglePoints.Decode(route.overview_polyline.points);

                    if (points != null)
                    {
                        var polyLine = new Maps.Polyline
                        {
                            StrokeColor = Color.Blue,
                            StrokeWidth = 6
                        };

                        foreach (var p in points)
                        {
                            polyLine.Geopath.Add(new Position(p.Latitude, p.Longitude));
                        }

                        MapReset();

                        Map.MapElements.Add(polyLine);

                        AddPinFromLocation(SelectedHotel, PsPinType.Hotel);

                        var extent = new MyExtent(route.bounds.northeast.lat, route.bounds.northeast.lng,
                                                  route.bounds.southwest.lat, route.bounds.southwest.lng);

                        var center = extent.GetCenter();
                        var dist = extent.GetDistance() * 0.6;

                        var mapspan = MapSpan.FromCenterAndRadius(center, Maps.Distance.FromKilometers(dist));

                        Map.MoveToRegion(mapspan);
                    }
                }
            }
        }
    }
}