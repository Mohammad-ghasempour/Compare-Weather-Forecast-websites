﻿using AutoMapper;
using System.Globalization;
using WeatherWebAPI.Contracts;
using WeatherWebAPI.Factory.Strategy;
using WeatherWebAPI.Factory.Strategy.OpenWeather;
using WeatherWebAPI.Query;

namespace WeatherWebAPI.DAL.Query
{
    public class GetWeatherForecastByWeekNumberQuery : BaseFunctionsForQueriesAndCommands
    {
        private readonly IMapper _mapper;
        private readonly IGetCitiesQuery _getCitiesQuery;
        private readonly IOpenWeatherFetchCityStrategy _openWeatherFetchCityStrategy;
        private readonly IGetWeatherDataFromDatabaseStrategy _getWeatherDataFromDatabaseStrategy;

        public GetWeatherForecastByWeekNumberQuery(
            IMapper mapper, 
            IGetCitiesQuery getCitiesQuery,
            IOpenWeatherFetchCityStrategy openWeatherFetchCityStrategy,
            IGetWeatherDataFromDatabaseStrategy getWeatherDataFromDatabaseStrategy
            ) : base()
        {
            _mapper = mapper;
            _getCitiesQuery = getCitiesQuery;
            _openWeatherFetchCityStrategy = openWeatherFetchCityStrategy;
            _getWeatherDataFromDatabaseStrategy = getWeatherDataFromDatabaseStrategy;
        }

        public async Task<List<WeatherForecastDto>> GetWeatherForecastByWeek(WeekQueryAndCity query)
        {
            string? citySearchedFor = query.CityQuery?.City;
            string? cityName;
            DateTime monday = FirstDateOfWeekISO8601(DateTime.UtcNow.Year, query.Week);
            DateTime sunday = monday.AddDays(6);

            var dtoList = new List<WeatherForecastDto>();
            var datesInWeek = new List<DateTime>();

            try
            {
                var cities = await _getCitiesQuery.GetAllCities();

                // Making sure the city names are in the right format (Capital Letter + rest of name, eg: Stavanger, not StAvAngeR)
                TextInfo textInfo = new CultureInfo("no", true).TextInfo;
                citySearchedFor = textInfo.ToTitleCase(citySearchedFor!);

                foreach (DateTime day in EachDay(monday, sunday))
                {
                    datesInWeek.Add(day);
                }

                if (!CityExists(citySearchedFor, cities))
                {
                    var cityData = await GetCityData(citySearchedFor, _openWeatherFetchCityStrategy);
                    cityName = cityData[0].Name;
                }
                else
                {
                    cityName = citySearchedFor;
                }

                string queryString = $"SET DATEFIRST 1 " +
                              $"SELECT WeatherData.Id, [Date], WeatherType, Temperature, Windspeed, WindspeedGust, WindDirection, Pressure, Humidity, ProbOfRain, AmountRain, CloudAreaFraction, FogAreaFraction, ProbOfThunder, DateForecast, " +
                                    $"City.[Name] as CityName, [Source].[Name] as SourceName, Score.Value, Score.ValueWeighted, Score.FK_WeatherDataId FROM WeatherData " +
                                        $"INNER JOIN City ON City.Id = WeatherData.FK_CityId " +
                                            $"INNER JOIN SourceWeatherData ON SourceWeatherData.FK_WeatherDataId = WeatherData.Id " +
                                                $"INNER JOIN[Source] ON SourceWeatherData.FK_SourceId = [Source].Id " +
                                                    $"FULL OUTER JOIN Score ON Score.FK_WeatherDataId = WeatherData.Id " +
                                                        $"WHERE CAST([DateForecast] as date) = CAST([Date] as date) AND DATEPART(week, [DateForecast]) = {query.Week} AND City.Name = '{cityName}' " +
                                                            $"ORDER BY [DateForecast], [Date]";

                await MakeWeatherForecastDto(_mapper, dtoList, queryString, _getWeatherDataFromDatabaseStrategy);
            }
            catch (Exception)
            {
                throw;
            }
            return dtoList;
        }
    }
}
