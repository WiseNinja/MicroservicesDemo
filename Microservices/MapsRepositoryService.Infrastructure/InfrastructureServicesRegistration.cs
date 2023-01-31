using MapsRepositoryService.Core.DB.Commands;
using MapsRepositoryService.Core.DB.Queries;
using MapsRepositoryService.Infrastructure.MinIO.Queries;
using Microsoft.Extensions.DependencyInjection;

namespace MapsRepositoryService.Infrastructure;

public static class InfrastructureServicesRegistration
{
    public static IServiceCollection AddRepositoryInfrastructureServices(this IServiceCollection services)
    {
        services.AddScoped<IGetAllMapsQuery, GetAllMapsQuery>();
        return services;
    }
}