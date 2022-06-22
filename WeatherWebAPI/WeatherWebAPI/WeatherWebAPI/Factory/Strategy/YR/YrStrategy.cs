﻿using System.Net.Http.Headers;
using System.Text.Json;
using WeatherWebAPI.Controllers;

namespace WeatherWebAPI.Factory.Strategy.YR
{
    public class YrStrategy : IGetWeatherDataStrategy<WeatherForecastDto>, IYrStrategy
    {
        private readonly YrConfig _yrConfig;

        public YrStrategy(YrConfig config)
        {

            _yrConfig = config;

        }

        public async Task<WeatherForecastDto> GetWeatherDataFrom(CityDto city, DateTime queryDate)
        {
            var httpClient = new HttpClient
            {
                BaseAddress = _yrConfig.BaseUrl
            };

            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows 10, Win64; x64; rv:100.0) Gecko/20100101 FireFox/100.0");

            var response = await httpClient.GetAsync($"complete?lat={city.Latitude}&lon={city.Longitude}");
            //var response = await httpClient.GetAsync(strategy.MakeUriWeatherCall(lat, lon));
            //var response = await httpClient.SendAsync(new HttpRequestMessage
            //{
            //    Method = HttpMethod.Get
            //});

            if (response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                var weatherData = JsonSerializer.Deserialize<ApplicationYr>(responseBody);

                // Mapper
                //TimeSpan ts = new(queryDate.Hour + 1, 0, 0); // Setting the query date to get the closest weatherforecast from when the call were made.
                //queryDate = queryDate.Date + ts;
                _yrConfig.Get(queryDate);


                var resultWeatherData = _yrConfig.MapperConfig.CreateMapper().Map<WeatherForecastDto>(weatherData);
                return resultWeatherData;
            }

            return new WeatherForecastDto();
        }

        public string GetDataSource()
        {
            return _yrConfig.DataSource!;
        }
    }
}