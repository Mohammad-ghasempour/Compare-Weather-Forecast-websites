﻿using AutoMapper;

namespace WeatherWebAPI.Factory
{
    public interface IFactory
    {
        dynamic Build<IGetWeatherDataStrategy>();
        IMapper Mapper { get; }
    }
}