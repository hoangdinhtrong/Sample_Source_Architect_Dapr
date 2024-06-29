using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using SampeDapr.Application.Shared.Dtos;
using SampeDapr.Application.Shared.Features;
using SampeDapr.Application.Shared.Interfaces;
using SampeDapr.Domain;

namespace SampeDapr.Application.Features
{
    public class GetAllWeatherForecastHandler : IRequestHandler<GetAllWeatherForecastRequest, List<WeatherForecastDto>?>
    {
        private readonly IMapper _mapper;
        private readonly ILogger<GetAllWeatherForecastHandler> _logger;
        private readonly IWeatherForecastRepository _weatherForecastRepo;

        public GetAllWeatherForecastHandler(IMapper mapper
            , ILogger<GetAllWeatherForecastHandler> logger
            , IWeatherForecastRepository weatherForecastRepo)
        {
            _mapper = mapper;
            _logger = logger;
            _weatherForecastRepo = weatherForecastRepo;
        }
        public async Task<List<WeatherForecastDto>?> Handle(GetAllWeatherForecastRequest request, CancellationToken cancellationToken)
        {
            try
            {
                List<WeatherForecast>? weatherForecasts = await _weatherForecastRepo.GetAllForecasts();
                return _mapper.Map<List<WeatherForecastDto>?>(weatherForecasts);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return null;
            }
        }
    }
}
