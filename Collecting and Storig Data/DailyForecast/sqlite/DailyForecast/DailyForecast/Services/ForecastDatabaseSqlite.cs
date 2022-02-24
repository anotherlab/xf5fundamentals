using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using DailyForecast.Models;
using SQLite;

namespace DailyForecast.Services
{
	public class ForecastDatabaseSqlite
	{
		private static SQLiteAsyncConnection Database;
		private const string DatabaseFilename = "Forecast.db3";

		private const SQLiteOpenFlags Flags =
				// open the database in read/write mode
				SQLiteOpenFlags.ReadWrite |
				// create the database if it doesn't exist
				SQLiteOpenFlags.Create |
				// enable multi-threaded database access
				SQLiteOpenFlags.SharedCache;

		public string DatabasePath
		{
			get
			{
				var basePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
				return Path.Combine(basePath, DatabaseFilename);
			}
		}

		public static readonly AsyncLazy<ForecastDatabaseSqlite> Instance = new AsyncLazy<ForecastDatabaseSqlite>(async () =>
		{
			var instance = new ForecastDatabaseSqlite();
			CreateTableResult result = await Database.CreateTableAsync<DisplayedTemperature>();
			return instance;
		});

		public ForecastDatabaseSqlite()
		{
			Database = new SQLiteAsyncConnection(DatabasePath, Flags);
		}

		public Task<List<DisplayedTemperature>> GetItemsAsync()
		{
			return Database.Table<DisplayedTemperature>().ToListAsync();
		}

		public Task<DisplayedTemperature> GetItemAsync(DateTime forecastDate)
		{
			return Database.Table<DisplayedTemperature>().Where(i => i.ForecastDate == forecastDate).FirstOrDefaultAsync();
		}

		public Task<DisplayedTemperature> GetItemAsync(int Id)
		{
			return Database.Table<DisplayedTemperature>().Where(i => i.Id == Id).FirstOrDefaultAsync();
		}

		public Task<int> UpsertAsync(DisplayedTemperature item)
		{
			if (item.Id != 0)
			{
				return Database.UpdateAsync(item);
			}
			else
			{
				return Database.InsertAsync(item);
			}
		}

		public Task<int> DeleteItemAsync(DisplayedTemperature item)
		{
			return Database.DeleteAsync(item);
		}

		public Task<int> PurgeOlderAsync(int days)
		{
			// Datetime values are stored as ticks
			return Database.ExecuteAsync("delete from DisplayedTemperature where ForecastDate < ?", DateTime.Now.AddDays(days * -1).Ticks);
		}
	}
}
