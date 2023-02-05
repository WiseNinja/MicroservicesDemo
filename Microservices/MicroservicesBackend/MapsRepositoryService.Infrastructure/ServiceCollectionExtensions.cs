using MapsRepositoryService.Core.DB.Commands;
using MapsRepositoryService.Core.DB.Queries;
using MapsRepositoryService.Infrastructure.MinIO.Commands;
using MapsRepositoryService.Infrastructure.MinIO.Queries;
using Microsoft.Extensions.DependencyInjection;
using Minio;
using Minio.AspNetCore;

namespace MapsRepositoryService.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPersistence(this IServiceCollection services)
    {
        services.AddScoped<IGetAllMapsQuery, GetAllMapsQuery>();
        services.AddScoped<IGetMapDataQuery, GetMapDataQuery>();
        services.AddScoped<IGetMissionMapNameQuery, GetMissionMapNameQuery>();
        services.AddScoped<IInsertMapCommand, InsertMapCommand>();
        services.AddScoped<IDeleteMapCommand, DeleteMapCommand>();
        services.AddScoped<ISetMissionMapCommand, SetMissionMapCommand>();
        services.AddMinio(options =>
        {
            options.Endpoint = "minio:9000";
            options.AccessKey = "minio";
            options.SecretKey = "minio123";
            options.ConfigureClient(async client =>
            {
                client.Build();
                var mapsBucketFound = await client.BucketExistsAsync(new BucketExistsArgs().WithBucket("maps-bucket"));
                var missionMapBucketFound = await client.BucketExistsAsync(new BucketExistsArgs().WithBucket("missionmap-bucket"));
                if (!mapsBucketFound)
                {
                    await client.MakeBucketAsync(new MakeBucketArgs().WithBucket("maps-bucket"));
                }
                if (!missionMapBucketFound)
                {
                    await client.MakeBucketAsync(new MakeBucketArgs().WithBucket("missionmap-bucket"));
                }
            });
        });
        return services;
    }
}