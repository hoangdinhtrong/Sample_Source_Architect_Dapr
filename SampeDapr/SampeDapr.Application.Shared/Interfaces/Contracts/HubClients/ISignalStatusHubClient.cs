using SampeDapr.Application.Shared.Dtos;

namespace SampeDapr.Application.Shared.Interfaces
{
    public interface ISignalStatusHubClient
    {
        public Task UpdateSingalStatus(SignalStatusDto signalStatus);
    }
}
