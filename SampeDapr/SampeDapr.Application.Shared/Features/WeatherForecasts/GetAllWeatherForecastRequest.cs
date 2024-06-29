using MediatR;
using SampeDapr.Application.Shared.Dtos;

namespace SampeDapr.Application.Shared.Features
{
    public class GetAllWeatherForecastRequest : IRequest<List<WeatherForecastDto>?>
    {

    }
}
