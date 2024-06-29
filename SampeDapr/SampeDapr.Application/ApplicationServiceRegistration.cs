using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace SampeDapr.Application
{
    public static class ApplicationServiceRegistration
    {
        public static IServiceCollection ConfigureApplicationServices(this IServiceCollection services, 
            IConfiguration configuration)
        {
            services.AddMediatR(Assembly.GetExecutingAssembly());
            return services;
        }
    }
}