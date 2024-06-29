using Microsoft.EntityFrameworkCore;
using SampeDapr.Domain;

namespace SampeDapr.Persistence.MySql
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(DbContextOptions).Assembly);
        }

        public DbSet<WeatherForecast> WeatherForecasts { get; set; } = null!;
    }
}