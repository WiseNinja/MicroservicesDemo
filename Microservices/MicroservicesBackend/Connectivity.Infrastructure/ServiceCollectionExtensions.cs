using Connectivity.Core;
using Connectivity.Infrastructure.RabbitMQ;
using Microsoft.Extensions.DependencyInjection;

namespace Connectivity.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddConnectivity(this IServiceCollection services)
    {
        services.AddSingleton<IPublisher, RabbitMqPublisher>();
        services.AddSingleton<ISubscriber, RabbitMqSubscriber>();
        return services;
    }
}