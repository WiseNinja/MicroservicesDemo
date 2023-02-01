using MapsRepositoryService.Core.DB.Commands;
using MapsRepositoryService.Core.DB.Queries;
using MapsRepositoryService.Infrastructure.MinIO.Commands;
using MapsRepositoryService.Infrastructure.MinIO.Helpers;
using MapsRepositoryService.Infrastructure.MinIO.Queries;
using Microsoft.Extensions.DependencyInjection;
using Minio.AspNetCore;

namespace MapsRepositoryService.Infrastructure;

public static class InfrastructureServicesRegistration
{
    public static IServiceCollection AddRepositoryInfrastructureServices(this IServiceCollection services)
    {
        services.AddScoped<IGetAllMapsQuery, GetAllMapsQuery>();
        services.AddScoped<IGetMapDataQuery, GetMapDataQuery>();
        services.AddScoped<IInsertMapCommand, InsertMapCommand>();
        services.AddScoped<IDeleteMapCommand, DeleteMapCommand>();
        services.AddScoped<ISetMissionMapCommand, SetMissionMapCommand>();
        services.AddScoped<FileOperationsHelper>();
        services.AddMinio(options =>
        {
            options.Endpoint = "minio:9000";
            options.AccessKey = "minio";
            options.SecretKey = "minio123";
            options.ConfigureClient(client =>
            {
                client.Build();
            });
        });
        return services;
    }
}