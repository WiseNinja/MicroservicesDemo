using Connectivity.Core;
using Infrastructure.RabbitMQ;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddConnectivity(this IServiceCollection services)
    {
        services.AddSingleton<IPublisher, RabbitMqPublisher>();
        services.AddSingleton<ISubscriber, RabbitMqSubscriber>();
        return services;
    }
}