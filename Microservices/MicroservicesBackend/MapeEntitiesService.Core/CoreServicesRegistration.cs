﻿using MapEntitiesService.Core.Features.MapPoints;
using MapEntitiesService.Core.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace MapEntitiesService.Core;

public static class CoreServicesRegistration
{
    public static IServiceCollection AddCoreServices(this IServiceCollection services)
    {
        services.AddTransient<IMapPointsService, MapPointsService>();
        return services;
    }
}