using Avalonia.Media.Imaging;
using OpenWeatherMap.Avalonia.Sample.Models;
using OpenWeatherMap.Standard;
using OpenWeatherMap.Standard.Enums;
using OpenWeatherMap.Standard.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace OpenWeatherMap.Avalonia.Sample.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        private string currentWeather;
        private Bitmap weatherIcon;
        private string currentTemperature;
        private string highTemperature;
        private string lowTemperature;
        private string windSpeed;
        private string windDirection;

        public string CurrentWeather
        {
            get => currentWeather;
            set
            {
                currentWeather = value;
                OnPropertyChanged("CurrentWeather");
            }
        }
        public Bitmap WeatherIcon
        {
            get => weatherIcon;
            set
            {
                weatherIcon = value;
                OnPropertyChanged("WeatherIcon");
            }
        }
        public string CurrentTemperature
        {
            get => currentTemperature;
            set
            {
                currentTemperature = value;
                OnPropertyChanged("CurrentTemperature");
            }
        }
        public string HighTemperature
        {
            get => highTemperature;
            set
            {
                highTemperature = value;
                OnPropertyChanged("HighTemperature");
            }
        }
        public string LowTemperature
        {
            get => lowTemperature;
            set
            {
                lowTemperature = value;
                OnPropertyChanged("LowTemperature");
            }
        }
        public string WindDirection
        {
            get => windDirection;
            set
            {
                windDirection = value;
                OnPropertyChanged("WindDirection");
            }
        }
        public string WindSpeed
        {
            get => windSpeed;
            set
            {
                windSpeed = value;
                OnPropertyChanged("WindSpeed");
            }
        }

        public MainWindowViewModel(IApiSettings apiSettings)
        {
            var current = new Current(apiSettings.ApiKey)
            {
                Languages = Languages.English,
                FetchIcons = true,
                ForecastTimestamps = 5
            };

            WeatherData data = new WeatherData();
            current.Units = WeatherUnits.Imperial;
            Task.Run(async () =>
            {
                data = await current.GetWeatherDataByZipAsync("32927", Countries.UnitedStates);
                CurrentWeather = $"Current Weather {data.Weathers.First().Description}";
                CurrentTemperature = $"Current Temperature {data.WeatherDayInfo.Temperature}";
                HighTemperature = $"High Temperature {data.WeatherDayInfo.MaximumTemperature}";
                LowTemperature = $"Low Temperature {data.WeatherDayInfo.MinimumTemperature}";
                WindSpeed = $"Wind Speed {data.Wind.Speed}";
                WindDirection = $"Wind Direction {data.Wind.Degree}";
                WeatherIcon = await ImageHelper.LoadFromWeb(new Uri($"https://openweathermap.org/img/wn/{data.Weathers.First().Icon}.png"));
            });
            Task.WaitAll();

        }
    }
}
