namespace SampeDapr.Application.Shared.Interfaces
{
    public interface IListResult<T>
    {
        IReadOnlyList<T>? Items { get; set; }
    }
}
