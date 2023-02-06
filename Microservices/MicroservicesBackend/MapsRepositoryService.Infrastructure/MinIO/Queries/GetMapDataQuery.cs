using MapsRepositoryService.Core.DB.Queries;
using Microsoft.Extensions.Logging;
using Minio;

namespace MapsRepositoryService.Infrastructure.MinIO.Queries;

internal class GetMapDataQuery : IGetMapDataQuery
{
    private readonly MinioClient _minioClient;
    private readonly ILogger<GetMapDataQuery> _logger;

    public GetMapDataQuery(MinioClient minioClient, ILogger<GetMapDataQuery> logger)
    {
        _minioClient = minioClient;
        _logger = logger;
    }

    public async Task<string> GetMapDataByNameAsync(string mapName)
    {
        var mapData = string.Empty;
        try
        {
            var args = new GetObjectArgs()
                .WithBucket("maps-bucket")
                .WithObject(mapName).WithCallbackStream(stream => mapData = ConvertToBase64(stream));
            await _minioClient.GetObjectAsync(args);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error occurred when fetching objects from maps-bucket, details {ex}");
        }
        return mapData;
    }

    private string ConvertToBase64(Stream stream)
    {
        using var memoryStream = new MemoryStream();
        stream.CopyTo(memoryStream);
        var bytes = memoryStream.ToArray();

        var base64 = Convert.ToBase64String(bytes);
        return base64;
    }
}