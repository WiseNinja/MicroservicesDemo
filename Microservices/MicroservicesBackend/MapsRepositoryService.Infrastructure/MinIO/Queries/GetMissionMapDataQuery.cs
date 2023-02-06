using MapsRepositoryService.Core.DB.Queries;
using Microsoft.Extensions.Logging;
using Minio;

namespace MapsRepositoryService.Infrastructure.MinIO.Queries;

internal class GetMissionMapDataQuery : IGetMissionMapDataQuery
{
    private readonly MinioClient _minioClient;
    private readonly ILogger<GetMissionMapDataQuery> _logger;

    public GetMissionMapDataQuery(MinioClient minioClient, ILogger<GetMissionMapDataQuery> logger)
    {
        _minioClient = minioClient;
        _logger = logger;
    }

    public async Task<string> GetMissionMapDataByNameAsync(string mapName)
    {
        var mapData = string.Empty;
        try
        {
            var args = new GetObjectArgs()
                .WithBucket("missionmap-bucket")
                .WithObject(mapName).WithCallbackStream(stream => mapData = stream.ConvertToBase64());
            await _minioClient.GetObjectAsync(args);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error occurred when fetching objects from missionmap-bucket, details {ex}");
        }
        return mapData;
    }
}