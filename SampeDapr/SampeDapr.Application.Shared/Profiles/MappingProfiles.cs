using AutoMapper;
using SampeDapr.Application.Shared.Dtos;
using SampeDapr.Domain;

namespace SampeDapr.Application.Shared.Profiles
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<WeatherForecast, WeatherForecastDto>().ReverseMap();
        }
    }
}
