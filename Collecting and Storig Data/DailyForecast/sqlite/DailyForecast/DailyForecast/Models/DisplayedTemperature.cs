using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace DailyForecast.Models
{
	public class DisplayedTemperature
	{
		[PrimaryKey, AutoIncrement]
		public int Id { get; set; }
		public DateTime ForecastDate { get; set; }
		public double TempMin { get; set; }
		public double TempMax { get; set; }
	}
}
