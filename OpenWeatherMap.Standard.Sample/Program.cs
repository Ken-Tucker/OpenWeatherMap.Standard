﻿using Microsoft.Extensions.Configuration;
using OpenWeatherMap.Standard.Enums;
using OpenWeatherMap.Standard.Models;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace OpenWeatherMap.Standard.Sample
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            var config = new ConfigurationBuilder()
                             .AddUserSecrets<Program>()
                             .Build();
            var key = config["Weather:ApiKey"];

            var current = new Current(key)
            {
                Languages = Languages.German,
                FetchIcons = true,
                ForecastTimestamps = 5
            };

            WeatherData data = null;
            ForecastData forecastData = null;

            data = await current.GetWeatherDataByZipAsync("32927", "us");


            Console.WriteLine(
                $"[zip code]: current temperature in area zip-coded 32927 US is: {data.WeatherDayInfo.Temperature}");


            data = await current.GetWeatherDataByCityNameAsync("berlin", "de");

            Console.WriteLine(
                    $"[city, country code]: current temperature in Berlin, Germany is: {data.WeatherDayInfo.Temperature}");


            data = await current.GetWeatherDataByCityNameAsync("baghdad");

            Console.WriteLine($"[city]: current temperature in Baghdad is: {data.WeatherDayInfo.Temperature}");


            data = await current.GetWeatherDataByCoordinatesAsync(-33.865143, 151.209900);

            Console.WriteLine($"[lat, lon]: current temperature in Sydney is: {data.WeatherDayInfo.Temperature}");


            await using var fs = new FileStream($"{data?.Weathers[0]?.Icon}.png", FileMode.Create);
            await fs.WriteAsync(data?.Weathers[0]?.IconData);



            forecastData = await current.GetForecastDataByCityNameAsync("Schnelsen");

            foreach (var weatherDayInfo in forecastData.WeatherData.Select(a => new
            {
                a.WeatherDayInfo,
                a.AcquisitionDateTime
            }))
                Console.WriteLine(
                    $"[forecast]: Forecast for Schnelsen, Germany at {weatherDayInfo.AcquisitionDateTime}, maximum temp: {weatherDayInfo.WeatherDayInfo.MaximumTemperature}, minimum temp: {weatherDayInfo.WeatherDayInfo.MinimumTemperature}");

            var forecastDataSync = current.GetForecastDataByCityName("schnelsen");
            Console.WriteLine(System.Text.Json.JsonSerializer.Serialize(forecastDataSync));
            var geolocations = await current.GetGeoLocationAsync("titusville", "fl", "usa");
            foreach (var location in geolocations)
            {

                Console.WriteLine($"{location.name},  {location.lat}, {location.lon}");
            }

            var airPollution = await current.GetAirPollution(geolocations[0].lat, geolocations[0].lon);
            Console.ReadLine();
        }
    }
}