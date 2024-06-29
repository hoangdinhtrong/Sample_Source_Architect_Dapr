using Dapr.Client;
using Microsoft.Extensions.Configuration;
using SampeDapr.Application.Shared.Dtos;
using SampeDapr.Application.Shared.Interfaces;

namespace SampeDapr.OutService.MasterDataServices
{
    public class CurrencyService : BaseClient, ICurrencyService
    {
        private string _appId;

        public CurrencyService(DaprClient client,
            IConfiguration configuration) : base(client)
        {
            _appId = configuration["Dapr:Masterdata:AppId"];
        }
        public async Task<List<CurrencyDto>?> GetAllCurrencyAsync()
        {
            return await GetAsync<List<CurrencyDto>?>("api/Currency/GetListAsync", _appId);
        }
    }
}
