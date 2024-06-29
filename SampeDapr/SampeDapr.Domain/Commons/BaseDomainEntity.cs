namespace SampeDapr.Domain
{
    public abstract class BaseDomainEntity<Tkey> : IBaseDomainEntity<Tkey>
    {
        public Tkey? Id { get; set; }
        public DateTime? DateCreated { get; set; }
        public string? CreatedBy { get; set; }
        public string? CreatedByFullName { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public string? LastModifiedBy { get; set; }
        public string? LastModifiedByFullName { get; set; }
        public bool IsDeleted { get; set; }
        public string? DeletedBy { get; set; }
        public DateTime? DateDeleted { get; set; }
        public bool IsActive { get; set; }
    }

    public interface IBaseDomainEntity<Tkey>
    {
        Tkey? Id { get; set; }
    }
}
