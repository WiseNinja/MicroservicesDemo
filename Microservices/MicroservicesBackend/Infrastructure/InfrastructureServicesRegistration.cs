using Connectivity;
using Infrastructure.RabbitMQ;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class InfrastructureServicesRegistration
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        services.AddSingleton<IPublisher, RabbitMqPublisher>();
        services.AddSingleton<ISubscriber, RabbitMqSubscriber>();
        return services;
    }
}