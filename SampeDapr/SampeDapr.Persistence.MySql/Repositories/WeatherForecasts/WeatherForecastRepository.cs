using Microsoft.EntityFrameworkCore;
using SampeDapr.Application.Shared.Interfaces;
using SampeDapr.Domain;

namespace SampeDapr.Persistence.MySql.Repositories
{
    public class WeatherForecastRepository : IWeatherForecastRepository
    {
        private readonly AppDbContext _context;

        public WeatherForecastRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<List<WeatherForecast>?> GetAllForecasts()
        {
            return await _context.WeatherForecasts.ToListAsync();
        }
    }
}
