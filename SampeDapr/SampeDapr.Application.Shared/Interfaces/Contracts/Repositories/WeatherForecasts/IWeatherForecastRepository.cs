using SampeDapr.Domain;

namespace SampeDapr.Application.Shared.Interfaces
{
    public interface IWeatherForecastRepository
    {
        Task<List<WeatherForecast>?> GetAllForecasts();
    }
}
