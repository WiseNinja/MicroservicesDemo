using Common.Connectivity;
using Common.Infrastructure.Logging;
using Common.Infrastructure.RabbitMQ;
using Common.Logging;
using Microsoft.Extensions.DependencyInjection;

namespace Common.Infrastructure
{
    public static class InfrastructureServicesRegistration
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
        {
            services.AddScoped<IPublisher, RabbitMqPublisher>();
            services.AddScoped<ISubscriber, RabbitMqSubscriber>();
            services.AddTransient<ILoggingService, SeqLoggingService>();
            return services;
        }
    }
}
