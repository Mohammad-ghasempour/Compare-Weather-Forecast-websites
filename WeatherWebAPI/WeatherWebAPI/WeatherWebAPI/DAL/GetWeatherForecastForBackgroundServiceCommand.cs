﻿using WeatherWebAPI.Controllers;
using WeatherWebAPI.Factory;
using WeatherWebAPI.Query;

namespace WeatherWebAPI.DAL
{
    public class GetWeatherForecastForBackgroundServiceCommand : BaseGetWeatherForecastCommands
    {
        public GetWeatherForecastForBackgroundServiceCommand(IConfiguration config, IFactory factory) : base(config, factory)
        {

        }

        public async Task GetWeatherForecastForAllCities(List<IGetWeatherDataStrategy<WeatherForecastDto>> weatherDataStrategies)
        {
            DateTime date = DateTime.Now;

            var getCitiesQuery = new GetCitiesQuery(_config);
            var getDatesQuery = new GetDatesForCityQuery(_config);

            try
            {
                _citiesDatabase = await getCitiesQuery.GetAllCities();

                if (date >= DateTime.Now.Date)
                {
                    foreach (var weatherStrategy in weatherDataStrategies)
                    {
                        foreach (var city in _citiesDatabase)
                        {
                            _datesDatabase = await getDatesQuery.GetDatesForCity(city.Name);

                            if (GetWeatherDataBy(date))
                            {
                                await GetWeatherDataAndAddToDatabase(date, weatherStrategy, city);

                            }
                            if (UpdateWeatherDataBy(date))
                            {
                                await GetWeatherDataAndUpdateDatabase(date, weatherStrategy, city);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
