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
        services.AddTransient<IGetAllMapsQuery, GetAllMapsQuery>();
        services.AddTransient<IGetMapDataQuery, GetMapDataQuery>();
        services.AddTransient<IGetMissionMapDataQuery, GetMissionMapDataQuery>();
        services.AddTransient<IGetMissionMapNameQuery, GetMissionMapNameQuery>();
        services.AddTransient<IInsertMapCommand, InsertMapCommand>();
        services.AddTransient<IDeleteMapCommand, DeleteMapCommand>();
        services.AddTransient<ISetMissionMapCommand, SetMissionMapCommand>();
        services.AddMinio(options =>
        {
            options.Endpoint = "minio:9000";
            options.AccessKey = "minio";
            options.SecretKey = "minio123";
            options.ConfigureClient(client =>
            {
                client.Build();
                var mapsBucketFound = client.BucketExistsAsync(new BucketExistsArgs().WithBucket("maps-bucket")).Result;
                var missionMapBucketFound = client.BucketExistsAsync(new BucketExistsArgs().WithBucket("missionmap-bucket")).Result;
                if (!mapsBucketFound)
                {
                    client.MakeBucketAsync(new MakeBucketArgs().WithBucket("maps-bucket"));
                }
                if (!missionMapBucketFound)
                {
                    client.MakeBucketAsync(new MakeBucketArgs().WithBucket("missionmap-bucket"));
                }
            });
        });
        return services;
    }
}