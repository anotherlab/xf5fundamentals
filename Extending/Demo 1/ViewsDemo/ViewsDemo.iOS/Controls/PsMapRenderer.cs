using CoreGraphics;
using MapKit;
using System;
using System.Collections.Generic;
using UIKit;
using ViewsDemo.Controls;
using ViewsDemo.iOS.Controls;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Maps.iOS;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(PsMap), typeof(PsMapRenderer))]
namespace ViewsDemo.iOS.Controls
{
    public class PsMapRenderer : MapRenderer
    {
        List<PsPin> PsPins;

        protected override void OnElementChanged(ElementChangedEventArgs<View> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null)
            {
                if (Control is MKMapView nativeMap)
                {
                    nativeMap.GetViewForAnnotation = null;
                }
            }

            if (e.NewElement != null)
            {
                var formsMap = (PsMap)e.NewElement;
                var nativeMap = Control as MKMapView;
                PsPins = formsMap.PsPins;

                nativeMap.GetViewForAnnotation = GetViewForAnnotation;
            }
        }
        protected override MKAnnotationView GetViewForAnnotation(MKMapView mapView, IMKAnnotation annotation)
        {
            MKAnnotationView annotationView = null;

            if (annotation is MKUserLocation)
                return null;

            var customPin = GetCustomPin(annotation as MKPointAnnotation);
            if (customPin == null)
            {
                throw new Exception("Custom pin not found");
            }

            annotationView = mapView.DequeueReusableAnnotation(customPin.Label);
            if (annotationView == null)
            {
                annotationView = new CustomMKAnnotationView(annotation, customPin.Label);

                var pin_name = "here_pin";

                if (customPin.PinType == PsPinType.Office)
                    pin_name = "pluralsight_pin";

                if (customPin.PinType == PsPinType.Hotel)
                    pin_name = "hotel_pin";

                annotationView.Image = UIImage.FromBundle(pin_name);
            }
            annotationView.CanShowCallout = true;

            return annotationView;
        }

        PsPin GetCustomPin(MKPointAnnotation annotation)
        {
            if (annotation == null)
                return null;
            var position = new Position(annotation.Coordinate.Latitude, annotation.Coordinate.Longitude);

            if (PsPins == null)
                return null;

            foreach (var pin in PsPins)
            {
                if (pin != null && pin.Position == position)
                {
                    return pin;
                }
            }
            return null;
        }

    }
    public class CustomMKAnnotationView : MKAnnotationView
    {
        public string Id { get; set; }

        public CustomMKAnnotationView(IMKAnnotation annotation, string id) : base(annotation, id)
        {

        }
    }
}