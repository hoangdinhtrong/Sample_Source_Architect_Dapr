using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace SampeDapr.Application.Shared
{
    public static class ApplicationSharedServiceRegistration
    {
        public static IServiceCollection ConfigureApplicationSharedServices(this IServiceCollection services, 
            IConfiguration configuration)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddSignalR(option =>
            {
                option.KeepAliveInterval = TimeSpan.FromSeconds(5);
            }).AddMessagePackProtocol();
            return services;
        }
    }
}
