using MapEntitiesService.Core.Interfaces;
using MapEntitiesService.Core.Logging;
using Microsoft.Extensions.DependencyInjection;

namespace MapEntitiesService.Core
{
    public static class CoreServicesRegistration
    {
        public static IServiceCollection AddCoreServices(this IServiceCollection services)
        {
            services.AddScoped<ILoggingService, LoggingService>();

            return services;
        }
    }
}
