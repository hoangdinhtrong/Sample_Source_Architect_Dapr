namespace SampeDapr.Application.Shared.Dtos
{
    public class WeatherForecastDto
    {
        public DateTime? Date { get; set; }
        public int? TemperatureC { get; set; }
        public int? TemperatureF => TemperatureC.HasValue ? 32 + (int)(TemperatureC / 0.5556) : null;
        public string? Summary { get; set; }
    }
}
