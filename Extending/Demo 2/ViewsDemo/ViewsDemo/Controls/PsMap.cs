using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms.Maps;

namespace ViewsDemo.Controls
{
    public enum PsPinType { Here, Office, Hotel }

    public class PsPin : Pin
    {
        public PsPinType PinType { get; set; } = PsPinType.Office;
    }

    public class PsMap : Map
    {
        public List<PsPin> PsPins { get; set; } = new List<PsPin>();

        public void AddPin(PsPin pin)
        {
            this.Pins.Add(pin);
            this.PsPins.Add(pin);
        }
    }
}
