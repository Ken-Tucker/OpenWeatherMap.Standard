using OpenWeatherMap.Standard.Enums;
using OpenWeatherMap.Standard.Extensions;
using OpenWeatherMap.Standard.Implementations;
using OpenWeatherMap.Standard.Interfaces;
using OpenWeatherMap.Standard.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OpenWeatherMap.Standard
{
    /// <summary>
    ///     Forecast class.
    /// </summary>
    public class Current
    {
        /// <summary>
        ///     The API_ROOT.
        /// </summary>
        private const string API_ROOT = "api.openweathermap.org/data";

        /// <summary>
        ///     The API version.
        /// </summary>
        private const string API_VERSION = "/2.5";

        /// <summary>
        ///     Weather request root.
        /// </summary>
        private const string WEATHER_REQUESTS_ROOT = "/weather";

        /// <summary>
        ///     Forecast request root.
        /// </summary>
        private const string FORECAST_REQUESTS_ROOT = "/forecast";

        /// <summary>
        ///     The root URL where the icons are located.
        /// </summary>
        private const string ICON_DATA_ROOT = "openweathermap.org/img/wn";


        private string appId = string.Empty;

        /// <summary>
        ///     Default constructor that uses the default IRestService implementation.
        /// </summary>
        /// <param name="appId">OWM app id</param>
        public Current(string appId)
        {
            if (string.IsNullOrEmpty(appId))
                throw new ArgumentNullException(nameof(appId), "AppId must NOT be null or empty string");
            this.appId = appId;
            Service = new RestServiceCaller();
        }

        /// <summary>
        ///     A constructor that allows you to set your default measurements system.
        /// </summary>
        /// <param name="appId">OWM app id</param>
        /// <param name="units">desired system</param>
        public Current(string appId, WeatherUnits units)
        {
            if (string.IsNullOrEmpty(appId))
                throw new ArgumentNullException(nameof(appId), "AppId must NOT be null or empty string");
            AppId = appId;
            Service = new RestServiceCaller();
            Units = units;
        }

        /// <summary>
        ///     A constructor that allows you to use your own IRestService implementation.
        /// </summary>
        /// <param name="appId">OWM app id</param>
        /// <param name="rest">your IRestService implementation</param>
        public Current(string appId, IRestService rest)
        {
            if (string.IsNullOrEmpty(appId))
                throw new ArgumentNullException(nameof(appId), "AppId must NOT be null or empty string");
            AppId = appId;
            Service = rest;
        }

        /// <summary>
        ///     A constructor that allows you to use your own IRestService implementation and set default measurements system and
        ///     language.
        /// </summary>
        /// <param name="appId">OWM app id</param>
        /// <param name="rest">your IRestService implementation</param>
        /// <param name="units">desired system</param>
        /// <param name="languages">desired language</param>
        public Current(string appId, IRestService rest, WeatherUnits units, Languages languages)
        {
            if (string.IsNullOrEmpty(appId))
                throw new ArgumentNullException(nameof(appId), "AppId must NOT be null or empty string");
            AppId = appId;
            Service = rest;
            Units = units;
            Languages = languages;
        }

        /// <summary>
        ///     The rest service to perform our web calls.
        /// </summary>
        public IRestService Service { get; set; }

        /// <summary>
        ///     The root URL where the icons are located.
        /// </summary>
        private string IconDataRootUrl => $"http{(UseHTTPS ? "s" : "")}://{ICON_DATA_ROOT}";

        /// <summary>
        ///     The OWM app id to be used.
        /// </summary>
        public string AppId
        {
            get => appId;
            set
            {
                if (string.IsNullOrEmpty(value))
                    throw new Exception("AppId must NOT be null or empty string");
                appId = value;
            }
        }

        /// <summary>
        ///     The measurements system to be used as the default unless specified.
        ///     <para />
        ///     its default is Metric
        /// </summary>
        public WeatherUnits Units { get; set; } = WeatherUnits.Metric;

        /// <summary>
        ///     Translation is applied for the "city name" and "description" fields.
        ///     <para />
        ///     Its default is English.
        /// </summary>
        public Languages Languages { get; set; } = Languages.English;

        /// <summary>
        ///     Indicate weather to call the API through HTTPS or not.
        ///     <para />
        ///     It's highly recommended to leave it true.
        /// </summary>
        public bool UseHTTPS { get; set; } = true;

        /// <summary>
        ///     Indicate if the image data of the icons should be fetched from the OWM-site.
        ///     <para />
        ///     Its default is 'false'.
        /// </summary>
        public bool FetchIcons { get; set; } = false;

        /// <summary>
        ///     The number of timestamps, which will be returned in the API response.
        ///     <para />
        ///     Its default (and maximum) is 40 which returns eight weather-conditions per day.
        /// </summary>
        public int ForecastTimestamps { get; set; } = 40;

        /// <summary>
        ///     Get the API call URL.
        /// </summary>
        /// <param name="query">query parameters and values. ex: city=baghdad</param>
        /// <param name="getForecastUrl">whether normal weather or forecast-data should be retrieved</param>
        /// <returns>string url</returns>
        internal string GetUrl(string query, bool getForecastUrl = false)
        {
            if (string.IsNullOrEmpty(query))
                throw new ArgumentNullException(nameof(query), "query can NOT be null or empty string");
            return
                $"http{(UseHTTPS ? "s" : "")}://{API_ROOT}{API_VERSION}{(getForecastUrl ? FORECAST_REQUESTS_ROOT : WEATHER_REQUESTS_ROOT)}?{query}{(getForecastUrl ? $"&cnt={ForecastTimestamps}" : "")}&units={Units}&appid={AppId}&lang={Languages.GetStringValue()}";
        }

        /// <summary>
        ///     Get the API call URL to get weather data by zip code.
        /// </summary>
        /// <param name="zipCode">zip code</param>
        /// <param name="countryCode">country code</param>
        /// <param name="getForecastUrl">determines if the weather-forecast url should be returned</param>
        /// <returns>string url</returns>
        internal string GetWeatherOrForecastDataByZipUrl(string zipCode, string countryCode, bool getForecastUrl)
        {
            if (string.IsNullOrEmpty(zipCode))
                throw new ArgumentNullException(nameof(zipCode), "zipCode can NOT be null or empty string");
            if (string.IsNullOrEmpty(countryCode))
                throw new ArgumentNullException(nameof(countryCode), "countryCode can NOT be null or empty string");
            return GetUrl($"zip={zipCode},{countryCode}", getForecastUrl);
        }

        /// <summary>
        ///     Get the API call URL to get weather data by zip code.
        /// </summary>
        /// <param name="zipCode">zip code</param>
        /// <param name="country">country</param>
        /// <param name="getForecastUrl">determines if the weather-forecast url should be returned</param>
        /// <returns>string url</returns>
        internal string GetWeatherOrForecastDataByZipUrl(string zipCode, Countries country, bool getForecastUrl)
        {
            var countryCode = country.GetStringValue();
            return GetWeatherOrForecastDataByZipUrl(zipCode, countryCode, getForecastUrl);
        }

        /// <summary>
        ///     Get the API call URL to get weather data by city name and country code.
        /// </summary>
        /// <param name="cityName">city name</param>
        /// <param name="countryCode">country code</param>
        /// <param name="getForecastUrl">determines if the weather-forecast url should be returned</param>
        /// <returns>string url</returns>
        internal string GetWeatherOrForecastDataByCityNameUrl(string cityName, string countryCode, bool getForecastUrl)
        {
            if (string.IsNullOrEmpty(cityName))
                throw new ArgumentNullException(nameof(cityName), "cityName can NOT be null or empty string");
            var q = $"q={cityName}";
            if (!string.IsNullOrEmpty(countryCode))
                q += $",{countryCode}";
            return GetUrl(q, getForecastUrl);
        }

        /// <summary>
        ///     Get the API call URL to get weather data by city name and country.
        /// </summary>
        /// <param name="cityName">city name</param>
        /// <param name="country">country</param>
        /// <param name="getForecastUrl">determines if the weather-forecast url should be returned</param>
        /// <returns>string url</returns>
        internal string GetWeatherOrForecastDataByCityNameUrl(string cityName, Countries country, bool getForecastUrl)
        {
            var countryCode = country.GetStringValue();
            return GetWeatherOrForecastDataByCityNameUrl(cityName, countryCode, getForecastUrl);
        }

        /// <summary>
        ///     Get the API call URL to get weather data by city id.
        /// </summary>
        /// <param name="cityId">city id</param>
        /// <param name="getForecastUrl">determines if the weather-forecast url should be returned</param>
        /// <returns>string url</returns>
        internal string GetWeatherOrForecastDataByCityIdUrl(int cityId, bool getForecastUrl)
        {
            return GetUrl($"id={cityId}", getForecastUrl);
        }

        internal string GetGeoLocationUrl(string city, string state, string country)
        {
            return $"http://api.openweathermap.org/geo/1.0/direct?q={city},{state},{country}&limit=5&appid={AppId}";
        }

        /// <summary>
        ///     Get the API call URL to get weather data by latitude and longitude.
        /// </summary>
        /// <param name="lat">latitude</param>
        /// <param name="lon"></param>
        /// <param name="getForecastUrl">determines if the weather-forecast url should be returned</param>
        /// <returns>string url</returns>
        internal string GetWeatherOrForecastDataByCoordsUrl(double lat, double lon, bool getForecastUrl)
        {
            return GetUrl($"lat={lat}&lon={lon}", getForecastUrl);
        }

        #region Public Weather-Functions

        /// <summary>
        ///     Get weather data by zip code.
        /// </summary>
        /// <param name="zipCode">zip code</param>
        /// <param name="countryCode">country code</param>
        /// <returns>
        ///     <see cref="WeatherData" />
        /// </returns>
        public async Task<WeatherData> GetWeatherDataByZipAsync(string zipCode, string countryCode)
        {
            var url = GetWeatherOrForecastDataByZipUrl(zipCode, countryCode, false);
            return await Service.GetAsync(url, IconDataRootUrl, FetchIcons);
        }

        /// <summary>
        ///     Get weather data by zip code.
        /// </summary>
        /// <param name="zipCode">zip code</param>
        /// <param name="country">country</param>
        /// <returns>
        ///     <see cref="WeatherData" />
        /// </returns>
        public async Task<WeatherData> GetWeatherDataByZipAsync(string zipCode, Countries country)
        {
            var url = GetWeatherOrForecastDataByZipUrl(zipCode, country, false);
            return await Service.GetAsync(url, IconDataRootUrl, FetchIcons);
        }

        /// <summary>
        ///     Get weather data by zip code.
        /// </summary>
        /// <param name="zipCode">zip code</param>
        /// <param name="countryCode">country code</param>
        /// <returns>Task of <see cref="WeatherData" /></returns>
        public Task<WeatherData> GetWeatherDataByZip(string zipCode, string countryCode)
        {
            var url = GetWeatherOrForecastDataByZipUrl(zipCode, countryCode, false);
            return Service.GetAsync(url, IconDataRootUrl, FetchIcons);
        }

        /// <summary>
        ///     Get weather data by zip code.
        /// </summary>
        /// <param name="zipCode">zip code</param>
        /// <param name="country">country</param>
        /// <returns>Task of <see cref="WeatherData" /></returns>
        public Task<WeatherData> GetWeatherDataByZip(string zipCode, Countries country)
        {
            var url = GetWeatherOrForecastDataByZipUrl(zipCode, country, false);
            return Service.GetAsync(url, IconDataRootUrl, FetchIcons);
        }

        /// <summary>
        ///     Get weather data by city name and country code.
        /// </summary>
        /// <param name="cityName">city name</param>
        /// <param name="countryCode">country code</param>
        /// <returns>Task of <see cref="WeatherData" /></returns>
        public async Task<WeatherData> GetWeatherDataByCityNameAsync(string cityName, string countryCode = "")
        {
            var url = GetWeatherOrForecastDataByCityNameUrl(cityName, countryCode, false);
            return await Service.GetAsync(url, IconDataRootUrl, FetchIcons);
        }

        /// <summary>
        ///     Get weather data by city name and country.
        /// </summary>
        /// <param name="cityName">city name</param>
        /// <param name="country">country</param>
        /// <returns>Task of <see cref="WeatherData" /></returns>
        public async Task<WeatherData> GetWeatherDataByCityNameAsync(string cityName, Countries country)
        {
            var url = GetWeatherOrForecastDataByCityNameUrl(cityName, country, false);
            return await Service.GetAsync(url, IconDataRootUrl, FetchIcons);
        }

        /// <summary>
        ///     Get weather data by city name and country code.
        /// </summary>
        /// <param name="cityName">city name</param>
        /// <param name="countryCode">country code</param>
        /// <returns>Task of <see cref="WeatherData" /></returns>
        public Task<WeatherData> GetWeatherDataByCityName(string cityName, string countryCode = "")
        {
            var url = GetWeatherOrForecastDataByCityNameUrl(cityName, countryCode, false);
            return Service.GetAsync(url, IconDataRootUrl, FetchIcons);
        }

        /// <summary>
        ///     Get weather data by city name and country.
        /// </summary>
        /// <param name="cityName">city name</param>
        /// <param name="country">country</param>
        /// <returns>Task of <see cref="WeatherData" /></returns>
        public Task<WeatherData> GetWeatherDataByCityName(string cityName, Countries country)
        {
            var url = GetWeatherOrForecastDataByCityNameUrl(cityName, country, false);
            return Service.GetAsync(url, IconDataRootUrl, FetchIcons);
        }

        /// <summary>
        ///     Get weather data by city id.
        /// </summary>
        /// <param name="cityId">city id</param>
        /// <returns>
        ///     <see cref="WeatherData" />
        /// </returns>
        public async Task<WeatherData> GetWeatherDataByCityIdAsync(int cityId)
        {
            var url = GetWeatherOrForecastDataByCityIdUrl(cityId, false);
            return await Service.GetAsync(url, IconDataRootUrl, FetchIcons);
        }

        /// <summary>
        ///     Get weather data by city id.
        /// </summary>
        /// <param name="cityId">city id</param>
        /// <returns>Task of <see cref="WeatherData" /></returns>
        public Task<WeatherData> GetWeatherDataByCityId(int cityId)
        {
            var url = GetWeatherOrForecastDataByCityIdUrl(cityId, false);
            return Service.GetAsync(url, IconDataRootUrl, FetchIcons);
        }

        /// <summary>
        ///     Get weather data by coordinates.
        /// </summary>
        /// <param name="lat">latitude</param>
        /// <param name="lon">longitude</param>
        /// <returns>
        ///     <see cref="WeatherData" />
        /// </returns>
        public async Task<WeatherData> GetWeatherDataByCoordinatesAsync(double lat, double lon)
        {
            var url = GetWeatherOrForecastDataByCoordsUrl(lat, lon, false);
            return await Service.GetAsync(url, IconDataRootUrl, FetchIcons);
        }

        /// <summary>
        ///     Get weather data by coordinates.
        /// </summary>
        /// <param name="lat">latitude</param>
        /// <param name="lon">longitude</param>
        /// <returns>Task of <see cref="WeatherData" /></returns>
        public Task<WeatherData> GetWeatherDataByCoordinates(double lat, double lon)
        {
            var url = GetWeatherOrForecastDataByCoordsUrl(lat, lon, false);
            return Service.GetAsync(url, IconDataRootUrl, FetchIcons);
        }

        /// <summary>
        ///     Get weather data by coordinates.
        /// </summary>/
        /// <param name="coordinates">coordinates object</param>
        /// <returns>Task of <see cref="WeatherData" /></returns>
        public async Task<WeatherData> GetWeatherDataByCoordinatesAsync(Coordinates coordinates)
        {
            return await GetWeatherDataByCoordinatesAsync(coordinates.Latitude, coordinates.Longitude);
        }

        /// <summary>
        ///     Get weather data by coordinates.
        /// </summary>
        /// <param name="coordinates">coordinates object</param>
        /// <returns>Task of <see cref="WeatherData" /></returns>
        public Task<WeatherData> GetWeatherDataByCoordinates(Coordinates coordinates)
        {
            return GetWeatherDataByCoordinates(coordinates.Latitude, coordinates.Longitude);
        }

        #endregion

        #region Public Forecast-Functions

        /// <summary>
        ///     Get forecast data by zip code.
        /// </summary>
        /// <param name="zipCode">zip code</param>
        /// <param name="countryCode">country code</param>
        /// <returns>Task of <see cref="ForecastData" /></returns>
        public async Task<ForecastData> GetForecastDataByZipAsync(string zipCode, string countryCode)
        {
            var url = GetWeatherOrForecastDataByZipUrl(zipCode, countryCode, true);
            return await Service.GetForecastAsync(url, IconDataRootUrl, FetchIcons);
        }

        /// <summary>
        ///     Get forecast data by zip code.
        /// </summary>
        /// <param name="zipCode">zip code</param>
        /// <param name="country">country</param>
        /// <returns>Task of <see cref="ForecastData" /></returns>
        public async Task<ForecastData> GetForecastDataByZipAsync(string zipCode, Countries country)
        {
            var url = GetWeatherOrForecastDataByZipUrl(zipCode, country, true);
            return await Service.GetForecastAsync(url, IconDataRootUrl, FetchIcons);
        }

        /// <summary>
        ///     Get forecast data by zip code.
        /// </summary>
        /// <param name="zipCode">zip code</param>
        /// <param name="countryCode">country code</param>
        /// <returns>
        ///     <see cref="ForecastData" />
        /// </returns>
        public ForecastData GetForecastDataByZip(string zipCode, string countryCode)
        {
            var url = GetWeatherOrForecastDataByZipUrl(zipCode, countryCode, true);
            return Task.Run(async () =>
                    await Service.GetForecastAsync(url, IconDataRootUrl, FetchIcons).ConfigureAwait(false)).GetAwaiter()
                .GetResult();
        }

        /// <summary>
        ///     Get forecast data by zip code.
        /// </summary>
        /// <param name="zipCode">zip code</param>
        /// <param name="country">country</param>
        /// <returns>
        ///     <see cref="ForecastData" />
        /// </returns>
        public ForecastData GetForecastDataByZip(string zipCode, Countries country)
        {
            var url = GetWeatherOrForecastDataByZipUrl(zipCode, country, true);
            return Task.Run(async () =>
                    await Service.GetForecastAsync(url, IconDataRootUrl, FetchIcons).ConfigureAwait(false)).GetAwaiter()
                .GetResult();
        }

        /// <summary>
        ///     Get forecast data by city name and country code.
        /// </summary>
        /// <param name="cityName">city name</param>
        /// <param name="countryCode">country code</param>
        /// <returns>Task of <see cref="ForecastData" /></returns>
        public async Task<ForecastData> GetForecastDataByCityNameAsync(string cityName, string countryCode = "")
        {
            var url = GetWeatherOrForecastDataByCityNameUrl(cityName, countryCode, true);
            return await Service.GetForecastAsync(url, IconDataRootUrl, FetchIcons);
        }

        /// <summary>
        ///     Get forecast data by city name and country.
        /// </summary>
        /// <param name="cityName">city name</param>
        /// <param name="country">country</param>
        /// <returns>Task of <see cref="ForecastData" /></returns>
        public async Task<ForecastData> GetForecastDataByCityNameAsync(string cityName, Countries country)
        {
            var url = GetWeatherOrForecastDataByCityNameUrl(cityName, country, true);
            return await Service.GetForecastAsync(url, IconDataRootUrl, FetchIcons);
        }

        /// <summary>
        ///     Get forecast data by city name and country code.
        /// </summary>
        /// <param name="cityName">city name</param>
        /// <param name="countryCode">country code</param>
        /// <returns>
        ///     <see cref="ForecastData" />
        /// </returns>
        public ForecastData GetForecastDataByCityName(string cityName, string countryCode = "")
        {
            var url = GetWeatherOrForecastDataByCityNameUrl(cityName, countryCode, true);
            return Task.Run(async () =>
                    await Service.GetForecastAsync(url, IconDataRootUrl, FetchIcons).ConfigureAwait(false)).GetAwaiter()
                .GetResult();
        }

        /// <summary>
        ///     Get forecast data by city name and country.
        /// </summary>
        /// <param name="cityName">city name</param>
        /// <param name="country">country</param>
        /// <returns>
        ///     <see cref="ForecastData" />
        /// </returns>
        public ForecastData GetForecastDataByCityName(string cityName, Countries country)
        {
            var url = GetWeatherOrForecastDataByCityNameUrl(cityName, country, true);
            return Task.Run(async () =>
                    await Service.GetForecastAsync(url, IconDataRootUrl, FetchIcons).ConfigureAwait(false)).GetAwaiter()
                .GetResult();
        }

        /// <summary>
        ///     Get forecast data by city id.
        /// </summary>
        /// <param name="cityId">city id</param>
        /// <returns>Task of <see cref="ForecastData" /></returns>
        public async Task<ForecastData> GetForecastDataByCityIdAsync(int cityId)
        {
            var url = GetWeatherOrForecastDataByCityIdUrl(cityId, true);
            return await Service.GetForecastAsync(url, IconDataRootUrl, FetchIcons);
        }

        /// <summary>
        ///     Get forecast data by city id.
        /// </summary>
        /// <param name="cityId">city id</param>
        /// <returns>
        ///     <see cref="ForecastData" />
        /// </returns>
        public ForecastData GetForecastDataByCityId(int cityId)
        {
            var url = GetWeatherOrForecastDataByCityIdUrl(cityId, true);
            return Task.Run(async () =>
                    await Service.GetForecastAsync(url, IconDataRootUrl, FetchIcons).ConfigureAwait(false)).GetAwaiter()
                .GetResult();
        }

        /// <summary>
        ///     Get forecast data by coordinates.
        /// </summary>
        /// <param name="lat">latitude</param>
        /// <param name="lon">longitude</param>
        /// <returns>Task of <see cref="ForecastData" /></returns>
        public async Task<ForecastData> GetForecastDataByCoordinatesAsync(double lat, double lon)
        {
            var url = GetWeatherOrForecastDataByCoordsUrl(lat, lon, true);
            return await Service.GetForecastAsync(url, IconDataRootUrl, FetchIcons);
        }

        /// <summary>
        ///     Get forecast data by coordinates.
        /// </summary>
        /// <param name="lat">latitude</param>
        /// <param name="lon">longitude</param>
        /// <returns>
        ///     <see cref="ForecastData" />
        /// </returns>
        public ForecastData GetForecastDataByCoordinates(double lat, double lon)
        {
            var url = GetWeatherOrForecastDataByCoordsUrl(lat, lon, true);
            return Service.GetForecastAsync(url, IconDataRootUrl, FetchIcons).GetAwaiter().GetResult();
        }

        /// <summary>
        ///     Get forecast data by coordinates.
        /// </summary>
        /// <param name="coordinates">coordinates object</param>
        /// <returns>
        ///     <see cref="ForecastData" />
        /// </returns>
        public async Task<ForecastData> GetForecastDataByCoordinatesAsync(Coordinates coordinates)
        {
            return await GetForecastDataByCoordinatesAsync(coordinates.Latitude, coordinates.Longitude);
        }

        /// <summary>
        ///     Get forecast data by coordinates.
        /// </summary>
        /// <param name="coordinates">coordinates object</param>
        /// <returns>
        ///     <see cref="ForecastData" />
        /// </returns>
        public ForecastData GetForecastDataByCoordinates(Coordinates coordinates)
        {
            return GetForecastDataByCoordinates(coordinates.Latitude, coordinates.Longitude);
        }

        #endregion

        public async Task<List<GeoLocation>> GetGeoLocationAsync(string city, string state, string country)
        {
            var url = GetGeoLocationUrl(city, state, country);
            return await Service.GetGeoLocationAsync(url);
        }

        public async Task<AirPollution> GetAirPollution(float lat, float lon)
        {
            {
                var url = GetAirPollutionUrl(lat, lon);
                return await Service.GetAirPollutionAsync(url);
            }
        }

        internal string GetAirPollutionUrl(float lat, float lon)
        {
            return $"https://api.openweathermap.org/data/2.5/air_pollution?lat={lat}&lon={lon}&appid={AppId}";
        }
    }
}
