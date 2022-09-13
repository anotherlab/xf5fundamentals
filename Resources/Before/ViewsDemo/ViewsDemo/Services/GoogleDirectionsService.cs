using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using ViewsDemo.Models;

namespace ViewsDemo.Services
{
    public class GoogleDirectionsService
    {
        HttpClient _client;

        public HttpClient Client
        {
            // create only once
            get
            {
                if (_client == null)
                {
                    _client = new HttpClient
                    {
                        BaseAddress = new Uri("https://maps.googleapis.com/maps/")
                    };
                    _client.DefaultRequestHeaders.Accept.Clear();
                    _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                }

                return _client;
            }
        }

        public string ApiKey { get; set; }

        public async Task<string> GetDirectionsRaw(double startLat, double startLon, double endLat, double endLon)
        {
            var preferences = "mode=driving&transit_routing_preference=less_driving";
            var url = $"api/directions/json?{preferences}&origin={startLat},{startLon}&destination={endLat},{endLon}&key={ApiKey}";

            var result = await Client.GetAsync(url).ConfigureAwait(false);

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

        public async Task<GoogleMapsDirectionsResults> GetDirections(double originLatitude, double originLongitude, double destinationLatitude, double destinationLongitude)
        {
            GoogleMapsDirectionsResults results = null;

            var jsonString = await GetDirectionsRaw(originLatitude, originLongitude, destinationLatitude, destinationLongitude);

            if (!string.IsNullOrEmpty(jsonString))
            {
                results = JsonSerializer.Deserialize<GoogleMapsDirectionsResults>(jsonString);
            }

            return results;
        }
    }
}
