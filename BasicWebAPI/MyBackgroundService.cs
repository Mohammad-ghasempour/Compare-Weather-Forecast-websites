﻿using BasicWebAPI.DAL;
using BasicWebAPI.Factory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BasicWebAPI
{
    public class MyBackgroundService : BackgroundService
    {

        private readonly IConfiguration config;

        public MyBackgroundService(IConfiguration config)
        {
            this.config = config;
        }


        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                //while (!stoppingToken.IsCancellationRequested)
                //{
                //    Console.WriteLine("BackgroundService doing work");

                //    var command = new GetWeatherForecastForBackgroundServiceCommand(config);
                //    await command.GetWeatherForecastForAllCities(new List<IStrategy> { new YrStrategy(), new OpenWeatherStrategy() });

                //    await Task.Delay(new TimeSpan(24, 0, 0)); // 24 hours delay
                //}
                //await Task.CompletedTask;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

        }
    }
}
