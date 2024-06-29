using Microsoft.AspNetCore.SignalR;
using SampeDapr.Application.Shared.Dtos;
using SampeDapr.Application.Shared.Interfaces;

namespace SampeDapr.Application.Services
{
    public class SignalStatusHubClient : Hub<ISignalStatusHubClient>
    {
        public async Task UpdateSingalStatus(SignalStatusDto signalStatus)
        {
            await Clients.All.UpdateSingalStatus(signalStatus);
        }
    }
}
