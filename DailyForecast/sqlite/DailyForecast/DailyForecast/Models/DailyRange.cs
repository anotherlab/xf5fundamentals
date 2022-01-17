using System;
using System.Collections.Generic;
using System.Text;

namespace DailyForecast.Models
{
	public class DailyRange
	{
		public DateTime ForecastDate { get; set; }
		public double LowTemp { get; set; }
		public double HighTemp { get; set; }
	}
}
