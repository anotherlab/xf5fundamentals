using Android.App;
using Android.Content;
using Android.Gms.Maps.Model;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ViewsDemo.Controls;
using ViewsDemo.Droid.Controls;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Maps.Android;

[assembly: ExportRenderer(typeof(PsMap), typeof(PsMapRenderer))]
namespace ViewsDemo.Droid.Controls
{
    public class PsMapRenderer : MapRenderer
    {
        public PsMapRenderer(Context context) : base(context) { }

        protected override MarkerOptions CreateMarker(Pin pin)
        {
            var marker = new MarkerOptions();
            marker.SetPosition(new LatLng(pin.Position.Latitude, pin.Position.Longitude));
            marker.SetTitle(pin.Label);
            marker.SetSnippet(pin.Address);

            if (pin.GetType() == typeof(PsPin))
            {
                var p = (PsPin)pin;

                int resourceID = Resource.Drawable.here_pin;

                if (p.PinType == PsPinType.Office)
                    resourceID = Resource.Drawable.pluralsight_pin;

                if (p.PinType == PsPinType.Hotel)
                    resourceID = Resource.Drawable.hotel_pin;

                marker.SetIcon(BitmapDescriptorFactory.FromResource(resourceID));
            }

            return marker;
        }
    }
}