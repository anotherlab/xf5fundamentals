using System;
using System.Collections.Generic;
using System.Text;

namespace DailyForecast.Models
{
	public class DisplayedTemperature
	{
		public int Id { get; set; }
		public DateTime ForecastDate { get; set; }
		public double TempMin { get; set; }
		public double TempMax { get; set; }
	}
}
