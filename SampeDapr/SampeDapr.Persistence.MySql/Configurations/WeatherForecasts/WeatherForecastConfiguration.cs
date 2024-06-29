using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SampeDapr.Domain;

namespace SampeDapr.Persistence.MySql.Configurations
{
    public class WeatherForecastConfiguration : IEntityTypeConfiguration<WeatherForecast>
    {
        public void Configure(EntityTypeBuilder<WeatherForecast> builder)
        {
            builder.Property(x => x.Id).IsRequired();
        }
    }
}
