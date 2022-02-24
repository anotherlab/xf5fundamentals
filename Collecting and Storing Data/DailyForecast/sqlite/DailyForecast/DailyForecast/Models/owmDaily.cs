using System;
using System.Collections.Generic;
using System.Text;

namespace DailyForecast.Models
{

    public class owmDaily
    {
        public owmList[] list { get; set; }
    }

    public class owmList
    {
        public int _dt;

        public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dateTime = dateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dateTime;
        }
        public int dt
        {
            get => _dt;
            set
            {
                _dt = value;
                LocalTime = UnixTimeStampToDateTime(_dt);
            }
        }
        public DateTime LocalTime { get; private set; }

        public owmMain main { get; set; }
    }

    public class owmMain
    {
        public float temp_min { get; set; }
        public float temp_max { get; set; }
    }

}
