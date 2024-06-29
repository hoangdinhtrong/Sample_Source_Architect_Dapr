using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SampeDapr.Application.Shared.Interfaces;
using SampeDapr.Persistence.MySql.Repositories;

namespace SampeDapr.Persistence.MySql
{
    public static class PersistenceServiceRegistration
    {
        public static IServiceCollection ConfigurePersistenceService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(op =>
            {
                string connectionString = configuration.GetConnectionString("RFI");
                MySqlServerVersion version = new(new Version(8, 0, 29));
                op.UseMySql(connectionString, version);

            });

            services.AddScoped<IWeatherForecastRepository, WeatherForecastRepository>();
            return services;

        }
    }
}
