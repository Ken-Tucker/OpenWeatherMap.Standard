using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;

namespace OpenWeatherMap.Standard.Models
{
    /// <summary>
    /// weatherdata model
    /// </summary>
    [Serializable]
    public class WeatherData : BaseModel
    {
        public WeatherData()
        {
            wind = new Wind();
            coords = new Coordinates();
            clouds = new Clouds();
            rain = new Rain();
            snow = new Snow();
            dayInfo = new DayInfo();
            weatherDayInfo = new WeatherDayInfo();
            @base = string.Empty;
            name = string.Empty;
            weathers = Array.Empty<Weather>();
        }
        private string @base, name;
        private int vis, id, statusCode;
        private double pop;

        private Coordinates coords;
        private Weather[] weathers;
        private WeatherDayInfo weatherDayInfo;
        private Wind wind;
        private Clouds clouds;
        private Rain rain;
        private Snow snow;
        private DateTime dt;
        private DayInfo dayInfo;

        /// <summary>
        /// coordinates
        /// </summary>
        [JsonProperty("coord")]
        public Coordinates Coordinates
        {
            get => coords;
            set => SetProperty(ref coords, value);
        }

        /// <summary>
        /// array of weather
        /// </summary>
        [JsonProperty("weather")]
        public Weather[] Weathers
        {
            get => weathers;
            set => SetProperty(ref weathers, value);
        }

        /// <summary>
        /// Base
        /// </summary>
        [JsonProperty("base")]
        public string Base
        {
            get => @base;
            set => SetProperty(ref @base, value);
        }

        /// <summary>
        /// Probability of precipitation
        /// <para />
        /// Only filled by forecast-api.
        /// </summary>
        [JsonProperty("pop")]
        public double Precipitation
        {
            get => pop;
            set => SetProperty(ref pop, value);
        }

        /// <summary>
        /// main weather day info
        /// </summary>
        [JsonProperty("main")]
        public WeatherDayInfo WeatherDayInfo
        {
            get => weatherDayInfo;
            set => SetProperty(ref weatherDayInfo, value);
        }

        /// <summary>
        /// visibility
        /// </summary>
        [JsonProperty("visibility")]
        public int Visibility
        {
            get => vis;
            set => SetProperty(ref vis, value);
        }

        /// <summary>
        /// wind info
        /// </summary>
        public Wind Wind
        {
            get => wind;
            set => SetProperty(ref wind, value);
        }

        /// <summary>
        /// clouds info
        /// </summary>
        public Clouds Clouds
        {
            get => clouds;
            set => SetProperty(ref clouds, value);
        }

        /// <summary>
        /// rain info
        /// </summary>
        public Rain Rain
        {
            get => rain;
            set => SetProperty(ref rain, value);
        }

        /// <summary>
        /// snow info
        /// </summary>
        public Snow Snow
        {
            get => snow;
            set => SetProperty(ref snow, value);
        }

        /// <summary>
        /// acquisition datetime
        /// </summary>
        [JsonConverter(typeof(UnixDateTimeConverter)), JsonProperty("dt")]
        public DateTime AcquisitionDateTime
        {
            get => dt;
            set => SetProperty(ref dt, value);
        }

        /// <summary>
        /// day info
        /// </summary>
        [JsonProperty("sys")]
        public DayInfo DayInfo
        {
            get => dayInfo;
            set => SetProperty(ref dayInfo, value);
        }

        /// <summary>
        /// Id
        /// </summary>
        public int Id
        {
            get => id;
            set => SetProperty(ref id, value);
        }

        /// <summary>
        /// name
        /// </summary>
        public string Name
        {
            get => name;
            set => SetProperty(ref name, value);
        }

        /// <summary>
        /// response code
        /// </summary>
        [JsonProperty("cod")]
        public int HttpStatusCode
        {
            get => statusCode;
            set => SetProperty(ref statusCode, value);
        }
    }

}
