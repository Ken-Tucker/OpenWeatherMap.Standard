using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using OpenWeatherMap.Standard.Implementations;
using OpenWeatherMap.Standard.Models;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace OpenWeatherMap.Standard.Core.Test.Implementations
{
    public class RestServiceCallerTests
    {
        private readonly Mock<HttpMessageHandler> _httpMessageHandlerMock;
        private readonly HttpClient _httpClient;
        private readonly RestServiceCaller _restServiceCaller;

        public RestServiceCallerTests()
        {
            _httpMessageHandlerMock = new Mock<HttpMessageHandler>();
            _httpClient = new HttpClient(_httpMessageHandlerMock.Object);
            RestServiceCaller._httpClient = _httpClient;
            _restServiceCaller = new RestServiceCaller();
        }

        [Fact]
        public async Task GetAsync_ReturnsWeatherData()
        {
            // Arrange
            var url = "http://example.com/weather";
            var weatherData = new WeatherData
            {
                Name = "Test City",
                AcquisitionDateTime = System.DateTime.Now,
                DayInfo = new DayInfo
                {
                    Id = 1,
                    Country = "Test Country",
                    Sunrise = System.DateTime.Now,
                    Sunset = System.DateTime.Now
                }
            };
            var json = JsonConvert.SerializeObject(weatherData);
            var mockContent = new StringContent(json);
            using (var mockResponse = new HttpResponseMessage(HttpStatusCode.OK) { Content = mockContent })
            {
                _httpMessageHandlerMock.Protected()
                    .Setup<Task<HttpResponseMessage>>(
                        "SendAsync",
                        ItExpr.IsAny<HttpRequestMessage>(),
                        ItExpr.IsAny<CancellationToken>())
                    .ReturnsAsync(mockResponse);
                // Act
                var result = await _restServiceCaller.GetAsync(url);
                // Assert
                Assert.NotNull(result);
                Assert.Equal("Test City", result.Name);
            }
        }

        [Fact]
        public async Task GetForecastAsync_ReturnsForecastData()
        {
            // Arrange
            var url = "http://example.com/forecast";
            var forecastData = new ForecastData { City = new City { Name = "Test City", Sunrise = System.DateTime.Now, Sunset = System.DateTime.Now } };
            var json = JsonConvert.SerializeObject(forecastData);

            _httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(json)
                });

            // Act
            var result = await _restServiceCaller.GetForecastAsync(url);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Test City", result.City.Name);
        }

        [Fact]
        public async Task GetGeoLocationAsync_ReturnsGeoLocationList()
        {
            // Arrange
            var url = "http://example.com/geolocation";
            var geoLocations = new List<GeoLocation> { new GeoLocation { name = "Test Location" } };
            var json = JsonConvert.SerializeObject(geoLocations);

            _httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(json)
                });

            // Act
            var result = await _restServiceCaller.GetGeoLocationAsync(url);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal("Test Location", result[0].name);
        }

        [Fact]
        public async Task GetAirPollutionAsync_ReturnsAirPollution()
        {
            // Arrange
            var url = "http://example.com/airpollution";
            var airPollution = new AirPollution { coord = new Coord { lat = 10, lon = 20 } };
            var json = JsonConvert.SerializeObject(airPollution);

            _httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(json)
                });

            // Act
            var result = await _restServiceCaller.GetAirPollutionAsync(url);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(10, result.coord.lat);
            Assert.Equal(20, result.coord.lon);
        }
    }
}


