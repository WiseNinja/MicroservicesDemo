using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MapEntitiesService.Core.Interfaces;
using MapEntitiesService.Infrastructure.MessageServiceClients;

namespace MapEntitiesService.Infrastructure
{
    public static class InfrastructureServicesRegistration
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
        {
            services.AddScoped<IMessageServiceClient, RabbitMqClient>();

            return services;
        }
    }
}
