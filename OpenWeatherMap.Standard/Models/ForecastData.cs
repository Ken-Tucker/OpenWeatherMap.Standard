using Newtonsoft.Json;
using System;

namespace OpenWeatherMap.Standard.Models
{
    /// <summary>
    ///     forecast-data model
    /// </summary>
    [Serializable]
    public class ForecastData : BaseModel
    {
        public ForecastData()
        {
            city = new City();
            weatherData = Array.Empty<WeatherData>();
        }
        private City city;
        private int cnt, statusCode;
        private WeatherData[] weatherData;

        /// <summary>
        ///     array of weather-data
        /// </summary>
        [JsonProperty("list")]
        public WeatherData[] WeatherData
        {
            get => weatherData;
            set => SetProperty(ref weatherData, value);
        }

        /// <summary>
        ///     information of current city
        /// </summary>
        public City City
        {
            get => city;
            set => SetProperty(ref city, value);
        }

        /// <summary>
        ///     response code
        /// </summary>
        [JsonProperty("cnt")]
        public int Count
        {
            get => cnt;
            set => SetProperty(ref cnt, value);
        }

        /// <summary>
        ///     response code
        /// </summary>
        [JsonProperty("cod")]
        public int HttpStatusCode
        {
            get => statusCode;
            set => SetProperty(ref statusCode, value);
        }
    }
}