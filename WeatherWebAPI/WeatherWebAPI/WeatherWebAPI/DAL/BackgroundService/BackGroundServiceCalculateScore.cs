﻿using System.Diagnostics;
using WeatherWebAPI.Controllers;
using WeatherWebAPI.Factory;
using WeatherWebAPI.Factory.Strategy.Database;
using WeatherWebAPI.Query;

namespace WeatherWebAPI.DAL.BackgroundService
{
    public class BackGroundServiceCalculateScore : BaseGetWeatherForecastCommands
    {
        private const double WEIGHT_TEMPERATURE = 0.3;
        private const double WEIGHT_PRESSURE = 0.2;
        private const double WEIGHT_HUMIDITY = 0.15;
        private const double WEIGHT_AMOUNT_RAIN = 0.1;
        private const double WEIGHT_PROB_OF_RAIN = 0.05;
        private const double WEIGHT_WIND_SPEED = 0.1;
        private const double WEIGHT_WIND_DIRECTION = 0.05;
        private const double WEIGHT_CLOUD_AREA_FRACTION = 0.05;
        private const double WEIGHT_SUM = WEIGHT_TEMPERATURE + WEIGHT_PRESSURE + WEIGHT_HUMIDITY + WEIGHT_AMOUNT_RAIN + WEIGHT_PROB_OF_RAIN +
            WEIGHT_WIND_SPEED + WEIGHT_WIND_DIRECTION + WEIGHT_CLOUD_AREA_FRACTION;


        public BackGroundServiceCalculateScore(IConfiguration config, IFactory factory) : base(config, factory)
        {

        }

        public async Task CalculateScore()
        {
            var getCitiesQuery = new GetCitiesQuery(_config);

            try
            {

                _citiesDatabase = await getCitiesQuery.GetAllCities();

                string getActualWeather = $"SELECT WeatherData.Id, [Date], WeatherType, Temperature, Windspeed, WindspeedGust, WindDirection, Pressure, Humidity, ProbOfRain, AmountRain, CloudAreaFraction, FogAreaFraction, ProbOfThunder, DateForecast, " +
                                                $"City.[Name] as CityName, [Source].[Name] as SourceName FROM WeatherData " +
                                                    $"INNER JOIN City ON City.Id = WeatherData.FK_CityId " +
                                                        $"INNER JOIN SourceWeatherData ON SourceWeatherData.FK_WeatherDataId = WeatherData.Id " +
                                                            $"INNER JOIN[Source] ON SourceWeatherData.FK_SourceId = [Source].Id " +
                                                                $"AND CAST(DateForecast as date) = CAST([Date] as date) AND City.Name = '{_citiesDatabase[0].Name}' " +
                                                                    $"AND[Date] BETWEEN DATEADD(day,-7, GETDATE()) AND GETDATE() " +
                                                                        $"ORDER BY[Date], SourceName";

                string getPredictedWeather = $"SELECT WeatherData.Id, [Date], WeatherType, Temperature, Windspeed, WindspeedGust, WindDirection, Pressure, Humidity, ProbOfRain, AmountRain, CloudAreaFraction, FogAreaFraction, ProbOfThunder, DateForecast, " +
                                                $"City.[Name] as CityName, [Source].[Name] as SourceName FROM WeatherData " +
                                                    $"INNER JOIN City ON City.Id = WeatherData.FK_CityId " +
                                                        $"INNER JOIN SourceWeatherData ON SourceWeatherData.FK_WeatherDataId = WeatherData.Id " +
                                                            $"INNER JOIN[Source] ON SourceWeatherData.FK_SourceId = [Source].Id " +
                                                                $"AND CAST(DateForecast as date) != CAST([Date] as date) AND City.Name = '{_citiesDatabase[0].Name}' " +
                                                                    $"AND[Date] BETWEEN DATEADD(day,-7, GETDATE()) AND GETDATE() " +
                                                                        $"ORDER BY[Date], SourceName";

                IGetWeatherDataFromDatabaseStrategy getWeatherDataFromDatabaseStrategy = _factory.Build<IGetWeatherDataFromDatabaseStrategy>();
                var ActualWeather = getWeatherDataFromDatabaseStrategy.Get(getActualWeather);
                var PredictedWeather = getWeatherDataFromDatabaseStrategy.Get(getPredictedWeather);
                
                //var subResult = ActualWeather
                //    .Where(i => PredictedWeather.Any(p => p.DateForecast == i.Date) && i.Source.DataProvider == "Yr")
                //    .ToList();

                //subResult.Sum(i => i.Pressure);

                foreach (var actual in ActualWeather)
                {
                    foreach (var predicted in PredictedWeather)
                    {
                        if(actual.Date == predicted.DateForecast && actual.Source.DataProvider == predicted.Source.DataProvider && actual.City == predicted.City)
                        {
                            var temperatureDifference = Math.Abs(actual.Temperature - predicted.Temperature);
                            var pressureDifference = Math.Abs(actual.Pressure - predicted.Pressure);
                            var humidityDifference = Math.Abs(actual.Humidity - predicted.Humidity);
                            var amountRainDifference = Math.Abs(actual.AmountRain - predicted.AmountRain);
                            var probOfRainDifference = Math.Abs(actual.ProbOfRain - predicted.ProbOfRain);
                            var windSpeedDifference = Math.Abs(actual.Windspeed - predicted.Windspeed);
                            var windDirectionDifference = Math.Abs(actual.WindDirection - predicted.WindDirection);
                            var cloudAreaFractionDifference = Math.Abs(actual.CloudAreaFraction - predicted.CloudAreaFraction);

                            var sumActual = SumWeatherScoreVariables(actual);
                            var sumPredicted = SumWeatherScoreVariables(predicted);
                            var difference = Math.Abs(sumActual - sumPredicted);

                            var score = Math.Round(CalculatePercentage(sumActual, difference), 2);
                            Console.WriteLine($"Score: {score}");

                            var weightedScore = CalculateWeightedScore(temperatureDifference, pressureDifference, humidityDifference, amountRainDifference,
                                probOfRainDifference, windSpeedDifference, windDirectionDifference, cloudAreaFractionDifference);
                            Console.WriteLine($"Weighted Score: {weightedScore}");

                            if(actual.WeatherForecastId == predicted.WeatherForecastId) // Unødvendig??
                            {
                                await AddScoreToDatabase(score, weightedScore, actual.WeatherForecastId);
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

        private static double CalculatePercentage(double sumActualWeather, double difference)
        {
            return (sumActualWeather - difference) / sumActualWeather * 100;
        }

        private static double SumWeatherScoreVariables(WeatherForecastDto forecast)
        {
            return Math.Abs(forecast.Temperature + forecast.Windspeed + forecast.WindDirection +
                                forecast.Pressure + forecast.Humidity + forecast.ProbOfRain + forecast.AmountRain +
                                    forecast.CloudAreaFraction);
        }

        private static double CalculateWeightedScore(double tempDiff, double pressureDiff, 
            double humidityDiff, double amountRainDiff, double probOfRainDiff,
                double windSpdDiff, double windDirDiff, double cloudAFDiff)
        {
            Debug.Assert(WEIGHT_SUM == 1000);
            var weightedScore = ((tempDiff * WEIGHT_TEMPERATURE) + (pressureDiff * WEIGHT_PRESSURE) + (humidityDiff * WEIGHT_HUMIDITY) + 
                        (amountRainDiff * WEIGHT_AMOUNT_RAIN) + (probOfRainDiff * WEIGHT_PROB_OF_RAIN) + (windSpdDiff * WEIGHT_WIND_SPEED) + 
                            (windDirDiff * WEIGHT_WIND_DIRECTION) + (cloudAFDiff * WEIGHT_CLOUD_AREA_FRACTION))
                                / (WEIGHT_SUM);

            return weightedScore;
        }
    }
}