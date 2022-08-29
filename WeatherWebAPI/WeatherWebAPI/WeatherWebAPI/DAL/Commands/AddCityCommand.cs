﻿using System.Net;
using System.Globalization;
using WeatherWebAPI.Contracts;
using WeatherWebAPI.Controllers;
using WeatherWebAPI.Factory;
using WeatherWebAPI.Query;

namespace WeatherWebAPI.DAL.Commands
{
    public class AddCityCommand : BaseFunctionsForQueriesAndCommands
    {
        public AddCityCommand(IConfiguration config, IFactory factory) : base(config, factory)
        {

        }

        public async Task AddCity(CityQuery query)
        {
            string? citySearchedFor = query.City;
            string? cityName;

            var getCitiesQueryDatabase = new GetCitiesQuery(_config);

            try
            {
                _citiesDatabase = await getCitiesQueryDatabase.GetAllCities();

                // Making sure the city names are in the right format (Capital Letter + rest of name, eg: Stavanger, not StAvAngeR)
                TextInfo textInfo = new CultureInfo("no", true).TextInfo;
                citySearchedFor = textInfo.ToTitleCase(citySearchedFor!);

                if (!CityExists(citySearchedFor!))
                {
                    var cityData = await GetCityData(citySearchedFor);
                    cityName = cityData[0].Name;

                    if (cityName != "")
                    {
                        if (!CityExists(cityName!))
                        {
                            await AddCityToDatabase(cityData);
                        }
                    }

                    //return new HttpResponseMessage(HttpStatusCode.OK)
                    //{
                    //    Content = new StringContent(
                    //        $"The city {cityName} has been added.")
                    //};
                }
                else
                {
                    cityName = citySearchedFor;

                    //return new HttpResponseMessage(HttpStatusCode.Found)
                    //{
                    //    Content = new StringContent(
                    //        $"The city {cityName} is already saved to the database.")
                    //};
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}