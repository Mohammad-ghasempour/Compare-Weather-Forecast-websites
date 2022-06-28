﻿using System.Globalization;
using WeatherWebAPI.Controllers;
using WeatherWebAPI.Factory;
using WeatherWebAPI.Factory.Strategy.Database;
using WeatherWebAPI.Factory.Strategy.OpenWeather;

namespace WeatherWebAPI.DAL
{
    public abstract class BaseGetWeatherForecastCommands
    {
        protected readonly IConfiguration _config;
        protected readonly IFactory _factory;
        protected List<CityDto>? _citiesDatabase;
        protected List<WeatherForecastDto>? _datesDatabase;

        public BaseGetWeatherForecastCommands(IConfiguration config, IFactory factory)
        {
            this._config = config;
            this._factory = factory;
        }

        protected async Task GetCityAndAddToDatabase(string? cityName)
        {
            IGetCityDataStrategy<CityDto> strategy = _factory.Build<IOpenWeatherStrategy>();
            var city = await strategy.GetCityDataFor(cityName!);

            IAddCityToDatabaseStrategy addCityToDatabaseStrategy = _factory.Build<IAddCityToDatabaseStrategy>();
            await addCityToDatabaseStrategy.Add(city);
        }

        protected async Task GetWeatherDataAndUpdateDatabase(DateTime date, IGetWeatherDataStrategy<WeatherForecastDto> weatherStrategy, CityDto city)
        {
            var weatherData = await weatherStrategy.GetWeatherDataFrom(city, date);

            IUpdateWeatherDataToDatabaseStrategy updateDatabaseStrategy = _factory.Build<IUpdateWeatherDataToDatabaseStrategy>();
            await updateDatabaseStrategy.Update(weatherData, city, date);
        }

        protected async Task GetWeatherDataAndAddToDatabase(DateTime date, IGetWeatherDataStrategy<WeatherForecastDto> weatherStrategy, CityDto city)
        {
            var weatherData = await weatherStrategy.GetWeatherDataFrom(city, date);

            IAddWeatherDataToDatabaseStrategy addToDatabaseStrategy = _factory.Build<IAddWeatherDataToDatabaseStrategy>();
            await addToDatabaseStrategy.Add(weatherData, city);
        }

        protected bool CityExists(string cityName)
        {
            return _citiesDatabase!.ToList().Any(c => c.Name!.Equals(cityName));
        }

        protected CityDto GetCityDtoBy(string cityName)
        {
            return _citiesDatabase!.Where(c => c.Name!.Equals(cityName)).First();
        }

        protected bool UpdateWeatherDataBy(DateTime date) // DateExist()
        {
            return _datesDatabase!.ToList().Any(d => d.Date.Date.Equals(date.Date));
        }

        protected bool GetWeatherDataBy(DateTime date) // !DateExists()
        {
            return !_datesDatabase!.ToList().Any(d => d.Date.Date.Equals(date.Date));
        }

        protected IEnumerable<DateTime> EachDay(DateTime from, DateTime thru) // Between dates
        {
            for (var day = from; day <= thru; day = day.AddDays(1)) // Add .Date if you don't want time to from and thru
                yield return day;
        }

        protected static DateTime FirstDateOfWeekISO8601(int year, int weekOfYear)
        {
            DateTime jan1 = new DateTime(year, 1, 1);
            int daysOffset = DayOfWeek.Thursday - jan1.DayOfWeek;

            // Use first Thursday in January to get first week of the year as
            // it will never be in Week 52/53
            DateTime firstThursday = jan1.AddDays(daysOffset);
            var cal = CultureInfo.CurrentCulture.Calendar;
            int firstWeek = cal.GetWeekOfYear(firstThursday, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);

            var weekNum = weekOfYear;
            // As we're adding days to a date in Week 1,
            // we need to subtract 1 in order to get the right date for week #1
            if (firstWeek == 1)
            {
                weekNum -= 1;
            }

            // Using the first Thursday as starting week ensures that we are starting in the right year
            // then we add number of weeks multiplied with days
            var result = firstThursday.AddDays(weekNum * 7);

            // Subtract 3 days from Thursday to get Monday, which is the first weekday in ISO8601
            return result.AddDays(-3);
        }
    }
}
