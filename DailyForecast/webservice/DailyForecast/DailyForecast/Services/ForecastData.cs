using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using DailyForecast.Models;

namespace DailyForecast.Services
{
    public enum ForecastUnits { standard, metric, imperial };

    public class ForecastData
    {
		HttpClient _client;

		public HttpClient Client
		{
			// create only once
			get
			{
				if (_client == null)
				{
					_client = new HttpClient();
				}

				return _client;
			}
		}

		public ForecastUnits Units { get; set; } = ForecastUnits.metric;

		public string ApiKey { get; set; } = "Sign up for a free API key at https://openweathermap.org/api";

		async Task<string> GetForecastRaw(string address)
		{
			var url = $"https://api.openweathermap.org/data/2.5/forecast?q={address}&appid={ApiKey}&units={Units}";

			var result = await Client.GetAsync(new Uri(url));

			if (result.StatusCode == System.Net.HttpStatusCode.OK)
			{
				var jsonData = await result.Content.ReadAsStringAsync();

				return jsonData;
			}
			else if (result.StatusCode == System.Net.HttpStatusCode.Unauthorized)
			{
				// This is where you would have some error logging about not having the right API key
			}
			else
			{
				// This is where you would have other error logging
			}

			return string.Empty;
		}

		public async Task<owmDaily> DailyForecast(string address)
		{
			owmDaily owmDaily = null;

			var jsonString = await GetForecastRaw(address);

			if (!string.IsNullOrEmpty(jsonString))
			{
				owmDaily = JsonSerializer.Deserialize<owmDaily>(jsonString);
			}

			return owmDaily;
		}
	}
}
