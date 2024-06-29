namespace SampeDapr.Application.Shared.Interfaces
{
    public interface IDaprPublishService
    {
        Task PublishAsync<T>(T data) where T : class;
    }
}
