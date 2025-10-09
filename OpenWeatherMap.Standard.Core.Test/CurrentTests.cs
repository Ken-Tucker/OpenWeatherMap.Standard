using Moq;
using OpenWeatherMap.Standard.Interfaces;
using OpenWeatherMap.Standard.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using OpenWeatherMap.Standard.Enums;
using Xunit;

namespace OpenWeatherMap.Standard.Core.Test
{
    public class CurrentTests
    {
        private const string AppId = "YOUR_API_KEY";

        [Fact]
        public void GetAirPollutionUrl_ValidCoordinates_ReturnsCorrectUrl()
        {
            // Arrange
            var current = new Current(AppId);
            float lat = 40.7128f;
            float lon = -74.0060f;

            // Act
            var result = current.GetAirPollutionUrl(lat, lon);

            // Assert
            Assert.Equal($"https://api.openweathermap.org/data/2.5/air_pollution?lat={lat}&lon={lon}&appid={AppId}", result);
        }

        [Fact]
        public void GetAirPollutionUrl_NegativeCoordinates_ReturnsCorrectUrl()
        {
            // Arrange
            var current = new Current(AppId);
            float lat = -33.8688f;
            float lon = 151.2093f;

            // Act
            var result = current.GetAirPollutionUrl(lat, lon);

            // Assert
            Assert.Equal($"https://api.openweathermap.org/data/2.5/air_pollution?lat={lat}&lon={lon}&appid={AppId}", result);
        }

        [Fact]
        public void GetAirPollutionUrl_ZeroCoordinates_ReturnsCorrectUrl()
        {
            // Arrange
            var current = new Current(AppId);
            float lat = 0f;
            float lon = 0f;

            // Act
            var result = current.GetAirPollutionUrl(lat, lon);

            // Assert
            Assert.Equal($"https://api.openweathermap.org/data/2.5/air_pollution?lat={lat}&lon={lon}&appid={AppId}", result);
        }

        [Fact]
        public void GetWeatherOrForecastDataByZipUrl_ValidInput_ReturnsCorrectUrl()
        {
            // Arrange
            var current = new Current(AppId);
            string zipCode = "12345";
            string countryCode = "US";
            bool getForecastUrl = false;

            // Act
            var result = current.GetWeatherOrForecastDataByZipUrl(zipCode, countryCode, getForecastUrl);

            // Assert
            Assert.Equal($"https://api.openweathermap.org/data/2.5/weather?zip={zipCode},{countryCode}&units=Metric&appid={AppId}&lang=en", result);
        }

        [Fact]
        public void GetWeatherOrForecastDataByZipUrl_ValidInputWithCountryCode_ReturnsCorrectUrl()
        {
            // Arrange
            var current = new Current(AppId);
            string zipCode = "12345";
            string countryCode = "US";
            bool getForecastUrl = false;

            var country = Countries.UnitedStates;
            
            // Act
            var result = current.GetWeatherOrForecastDataByZipUrl(zipCode, country, getForecastUrl);
            
            // Assert
            Assert.Equal($"https://api.openweathermap.org/data/2.5/weather?zip={zipCode},{countryCode}&units=Metric&appid={AppId}&lang=en", result);
        }

        [Fact]
        public void GetWeatherOrForecastDataByCityNameUrl_ValidInput_ReturnsCorrectUrl()
        {
            // Arrange
            var current = new Current(AppId);
            string cityName = "New York";
            string countryCode = "US";
            bool getForecastUrl = false;

            // Act
            var result = current.GetWeatherOrForecastDataByCityNameUrl(cityName, countryCode, getForecastUrl);

            // Assert
            Assert.Equal($"https://api.openweathermap.org/data/2.5/weather?q={cityName},{countryCode}&units=Metric&appid={AppId}&lang=en", result);
        }

        [Fact]
        public void GetWeatherOrForecastDataByCityNameUrl_ValidInputWithCountryCode_ReturnsCorrectUrl()
        {
            // Arrange
            var current = new Current(AppId);
            string cityName = "New York";
            string countryCode = "US";
            bool getForecastUrl = false;
            
            var country = Countries.UnitedStates;
            
            // Act
            var result = current.GetWeatherOrForecastDataByCityNameUrl(cityName, country, getForecastUrl);
            
            // Assert
            Assert.Equal($"https://api.openweathermap.org/data/2.5/weather?q={cityName},{countryCode}&units=Metric&appid={AppId}&lang=en", result);
        }

        [Fact]
        public void GetWeatherOrForecastDataByCityIdUrl_ValidInput_ReturnsCorrectUrl()
        {
            // Arrange
            var current = new Current(AppId);
            int cityId = 5128581;
            bool getForecastUrl = false;

            // Act
            var result = current.GetWeatherOrForecastDataByCityIdUrl(cityId, getForecastUrl);

            // Assert
            Assert.Equal($"https://api.openweathermap.org/data/2.5/weather?id={cityId}&units=Metric&appid={AppId}&lang=en", result);
        }

        [Fact]
        public void GetWeatherOrForecastDataByZipUrl_WithForecast_ReturnsCorrectUrl()
        {
            // Arrange
            var current = new Current(AppId);
            string zipCode = "12345";
            string countryCode = "US";
            bool getForecastUrl = true;

            // Act
            var result = current.GetWeatherOrForecastDataByZipUrl(zipCode, countryCode, getForecastUrl);

            // Assert
            Assert.Equal($"https://api.openweathermap.org/data/2.5/forecast?zip={zipCode},{countryCode}&cnt=40&units=Metric&appid={AppId}&lang=en", result);
        }

        [Fact]
        public void GetWeatherOrForecastDataByCityNameUrl_WithForecast_ReturnsCorrectUrl()
        {
            // Arrange
            var current = new Current(AppId);
            string cityName = "New York";
            string countryCode = "US";
            bool getForecastUrl = true;

            // Act
            var result = current.GetWeatherOrForecastDataByCityNameUrl(cityName, countryCode, getForecastUrl);

            // Assert
            Assert.Equal($"https://api.openweathermap.org/data/2.5/forecast?q={cityName},{countryCode}&cnt=40&units=Metric&appid={AppId}&lang=en", result);
        }

        [Fact]
        public void GetWeatherOrForecastDataByCityIdUrl_WithForecast_ReturnsCorrectUrl()
        {
            // Arrange
            var current = new Current(AppId);
            int cityId = 5128581;
            bool getForecastUrl = true;

            // Act
            var result = current.GetWeatherOrForecastDataByCityIdUrl(cityId, getForecastUrl);

            // Assert
            Assert.Equal($"https://api.openweathermap.org/data/2.5/forecast?id={cityId}&cnt=40&units=Metric&appid={AppId}&lang=en", result);
        }

        [Fact]
        public void GetGeoLocationUrl_ValidInput_ReturnsCorrectUrl()
        {
            // Arrange
            var current = new Current(AppId);
            string city = "New York";
            string state = "NY";
            string country = "US";

            // Act
            var result = current.GetGeoLocationUrl(city, state, country);

            // Assert
            Assert.Equal($"http://api.openweathermap.org/geo/1.0/direct?q={city},{state},{country}&limit=5&appid={AppId}", result);
        }

        [Fact]
        public void GetGeoLocationUrl_ValidInputWithCountryCode_ReturnsCorrectUrl()
        {
            // Arrange
            var current = new Current(AppId);
            string city = "New York";
            string state = "NY";
            string countryCode = "US";

            var country = Countries.UnitedStates;
            
            // Act
            var result = current.GetGeoLocationUrl(city, state, country);
            
            // Assert
            Assert.Equal($"http://api.openweathermap.org/geo/1.0/direct?q={city},{state},{countryCode}&limit=5&appid={AppId}", result);
        }
        
        [Fact]
        public void GetWeatherOrForecastDataByCoordsUrl_ValidInput_ReturnsCorrectUrl()
        {
            // Arrange
            var current = new Current(AppId);
            double lat = 40.7128;
            double lon = -74.0060;
            bool getForecastUrl = false;

            // Act
            var result = current.GetWeatherOrForecastDataByCoordsUrl(lat, lon, getForecastUrl);

            // Assert
            Assert.Equal($"https://api.openweathermap.org/data/2.5/weather?lat={lat}&lon={lon}&units=Metric&appid={AppId}&lang=en", result);
        }

        [Fact]
        public async Task GetWeatherDataByZipAsync_ValidInput_ReturnsWeatherData()
        {
            // Arrange
            var mockService = new Mock<IRestService>();
            var expectedWeatherData = new WeatherData();
            mockService.Setup(service => service.GetAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>()))
                       .ReturnsAsync(expectedWeatherData);

            var current = new Current(AppId, mockService.Object);
            string zipCode = "12345";
            string countryCode = "US";

            // Act
            var result = await current.GetWeatherDataByZipAsync(zipCode, countryCode);

            // Assert
            Assert.Equal(expectedWeatherData, result);
        }

        [Fact]
        public async Task GetWeatherDataByCityNameAsync_ValidInput_ReturnsWeatherData()
        {
            // Arrange
            var mockService = new Mock<IRestService>();
            var expectedWeatherData = new WeatherData();
            mockService.Setup(service => service.GetAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>()))
                       .ReturnsAsync(expectedWeatherData);

            var current = new Current(AppId, mockService.Object);
            string cityName = "New York";
            string countryCode = "US";

            // Act
            var result = await current.GetWeatherDataByCityNameAsync(cityName, countryCode);

            // Assert
            Assert.Equal(expectedWeatherData, result);
        }

        [Fact]
        public async Task GetGeoLocationAsync_ValidInput_ReturnsGeoLocationList()
        {
            // Arrange
            var mockService = new Mock<IRestService>();
            var expectedGeoLocations = new List<GeoLocation> { new GeoLocation { name = "New York" } };
            mockService.Setup(service => service.GetGeoLocationAsync(It.IsAny<string>()))
                       .ReturnsAsync(expectedGeoLocations);

            var current = new Current(AppId, mockService.Object);
            string city = "New York";
            string state = "NY";
            string country = "US";

            // Act
            var result = await current.GetGeoLocationAsync(city, state, country);

            // Assert
            Assert.Equal(expectedGeoLocations, result);
        }

    }
}
