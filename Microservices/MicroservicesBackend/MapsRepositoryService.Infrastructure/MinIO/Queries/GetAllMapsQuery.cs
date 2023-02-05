using MapsRepositoryService.Core.DB.Queries;
using Microsoft.Extensions.Logging;
using Minio;
using Minio.DataModel;

namespace MapsRepositoryService.Infrastructure.MinIO.Queries;

internal class GetAllMapsQuery : IGetAllMapsQuery
{
    private readonly MinioClient _minioClient;
    private readonly ILogger<GetAllMapsQuery> _logger;

    public GetAllMapsQuery(MinioClient _minioClient, ILogger<GetAllMapsQuery> logger)
    {
        this._minioClient = _minioClient;
        _logger = logger;
    }
    public async Task<List<string>?> GetAllMapNamesAsync()
    {
        List<string>? allMapNames = null;

        try
        {
            allMapNames = new List<string>();
            TaskCompletionSource<object> doneFetchingMapNames = new TaskCompletionSource<object>();
            var listArgs = new ListObjectsArgs()
                .WithBucket("maps-bucket");

            IObservable<Item>? observable = _minioClient.ListObjectsAsync(listArgs);
            observable.Subscribe(
                item => allMapNames.Add(item.Key),
                ex => _logger.LogError(
                    $"A MinIO exception occurred during fetching of all file names from maps-bucket, details: {ex}"),
                () => { doneFetchingMapNames.SetResult(new object()); });
            await doneFetchingMapNames.Task;
        }
        catch (Exception ex)
        {
            _logger.LogError($"An error occurred when trying to fetch all Map names, details: {ex}");
        }
        return allMapNames;
    }
}