using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SampeDapr.Application.Shared.Interfaces;
using SampeDapr.OutService.MasterDataServices;

namespace SampeDapr.OutService
{
    public static class OutServicesRegistration
    {
        public static IServiceCollection ConfigOutService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDaprClient(e => e.UseHttpEndpoint(configuration["Dapr:Host"])
                                         .UseGrpcEndpoint(configuration["Dapr:Grpc"])
                                         .UseJsonSerializationOptions(new System.Text.Json.JsonSerializerOptions()
                                         {

                                         }));

            services.AddScoped<ICurrencyService, CurrencyService>();

            return services;
        }
    }
}
