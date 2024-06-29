using SampeDapr.Application.Shared.Dtos;

namespace SampeDapr.Application.Shared.Interfaces
{
    public interface ICurrencyService
    {
        Task<List<CurrencyDto>?> GetAllCurrencyAsync();
    }
}
