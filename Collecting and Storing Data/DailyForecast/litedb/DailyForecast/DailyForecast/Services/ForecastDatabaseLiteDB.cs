using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using DailyForecast.Models;
using LiteDB;

namespace DailyForecast.Services
{
    public class ForecastDatabaseLiteDB
    {
		private ILiteCollection<DisplayedTemperature> displayedTemperatureCollection;
		private LiteDatabase db;
		private const string DatabaseFilename = "Forecast.db";

		public string DatabasePath
		{
			get
			{
				var basePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
				return Path.Combine(basePath, DatabaseFilename);
			}
		}

		public static readonly AsyncLazy<ForecastDatabaseLiteDB> Instance = new AsyncLazy<ForecastDatabaseLiteDB>(() =>
		{
			var instance = new ForecastDatabaseLiteDB();
			return instance;
		});

		public ForecastDatabaseLiteDB()
		{
			// Instantiate a new instance of the database
			db = new LiteDatabase($"FileName={DatabasePath};Connection=Direct");

			// the collection instance
			displayedTemperatureCollection = db.GetCollection<DisplayedTemperature>("DisplayedTemperatures");

			// Create an index over the ForecastDate field
			displayedTemperatureCollection.EnsureIndex(x => x.ForecastDate, false);
		}

		public List<DisplayedTemperature> GetAll()
		{
			var displayedTemperatures = new List<DisplayedTemperature>(displayedTemperatureCollection.FindAll());

			return displayedTemperatures;
		}

		public void Upsert(DisplayedTemperature displayedTemperature)
		{
			if (displayedTemperature.Id > 0)
			{
				displayedTemperatureCollection.Update(displayedTemperature);
			}
			else
			{
				displayedTemperatureCollection.Insert(displayedTemperature);
			}
		}

		public DisplayedTemperature Get(DateTime forecastDate)
		{
			var d = GetAll().Where(x => x.ForecastDate == forecastDate).FirstOrDefault();
			return d;
		}

		public void PurgeOlder(int days)
		{
			var purgeDate = DateTime.Now.AddDays(days * -1);

			var d = GetAll().Where(x => x.ForecastDate <= purgeDate);

			db.BeginTrans();

			foreach (var item in d)
			{
				displayedTemperatureCollection.Delete(item.Id);
			}

			db.Commit();
		}

		public bool BeginTrans()
		{
			return db.BeginTrans();
		}

		public bool Commit()
		{
			return db.Commit();
		}
	}
}
