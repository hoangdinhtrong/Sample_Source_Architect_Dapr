namespace SampeDapr.Domain
{
    public class EmailTemplate : BaseDomainEntity<long>
    {
        public string? Name { get; set; }
        public string? Subject { get; set; }
        public string? Body { get; set; }
    }
}
