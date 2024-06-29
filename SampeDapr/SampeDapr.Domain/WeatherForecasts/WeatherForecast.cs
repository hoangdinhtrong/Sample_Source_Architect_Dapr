namespace SampeDapr.Domain
{
    public class WeatherForecast : BaseDomainEntity<long>
    {
        public DateTime? Date { get; set; }
        public int? TemperatureC { get; set; }
        public string? Summary { get; set; }
    }
}
