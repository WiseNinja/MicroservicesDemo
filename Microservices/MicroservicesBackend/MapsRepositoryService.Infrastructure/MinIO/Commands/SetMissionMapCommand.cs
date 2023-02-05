using MapsRepositoryService.Core.DB.Commands;
using Microsoft.Extensions.Logging;
using Minio;
using Minio.DataModel;
using Minio.Exceptions;

namespace MapsRepositoryService.Infrastructure.MinIO.Commands;

internal class SetMissionMapCommand : ISetMissionMapCommand
{
    private readonly MinioClient _minioClient;
    private readonly ILogger<SetMissionMapCommand> _logger;

    public SetMissionMapCommand(MinioClient _minioClient, ILogger<SetMissionMapCommand> logger)
    {
        this._minioClient = _minioClient;
        _logger = logger;
    }

    public async Task<bool> SetMainMissionMapAsync(string? mapName)
    {
        List<string>? missionMapToDeleteInList = await GetAllFileNamesFromBucketAsync("missionmap-bucket");
        if (missionMapToDeleteInList is null)
        {
            return false;
        }
        if (missionMapToDeleteInList.Any())
        {
            var clearBucketContentsWasSuccessful = await ClearBucketContentsAsync("missionmap-bucket", missionMapToDeleteInList);
            if (!clearBucketContentsWasSuccessful)
            {
                return false;
            }
        }

        try
        {
            var cpSrcArgs = new CopySourceObjectArgs()
                .WithBucket("maps-bucket")
                .WithObject(mapName);

            var args = new CopyObjectArgs()
                .WithBucket("missionmap-bucket")
                .WithObject(mapName)
                .WithCopyObjectSource(cpSrcArgs);

            await _minioClient.CopyObjectAsync(args);
        }
        catch (Exception ex)
        {
            _logger.LogError($"An error occured when trying to copy the Mission Map to its bucket, details: {ex}");
            return false;
        }

        return true;
    }

    private async Task<bool> ClearBucketContentsAsync(string destBucketName, List<string> filesToDelete)
    {
        try
        {
            var objArgs = new RemoveObjectsArgs()
                .WithBucket(destBucketName)
                .WithObjects(filesToDelete);
            IObservable<DeleteError>? objectsOservable =
                await _minioClient.RemoveObjectsAsync(objArgs).ConfigureAwait(false);
            objectsOservable.Subscribe(
                objDeleteError => _logger.LogInformation($"Object: {objDeleteError.Key}"),
                ex => _logger.LogError($"A MinIO exception occurred during object deletion: {ex}"),
                () => { _logger.LogInformation($"Removed objects in list from {destBucketName}\n"); });
        }
        catch (Exception)
        {
            return false;
        }

        return true;
    }

    private async Task<List<string>?> GetAllFileNamesFromBucketAsync(string bucketName)
    {
        List<string>? allFileNames = null;

        TaskCompletionSource<object> doneFetchingFileNames = new TaskCompletionSource<object>();
        var listArgs = new ListObjectsArgs()
            .WithBucket(bucketName);
        try
        {
            allFileNames = new List<string>();
            IObservable<Item>? observable = _minioClient.ListObjectsAsync(listArgs);
            observable.Subscribe(
                item => allFileNames.Add(item.Key),
                ex => _logger.LogError(
                    $"A MinIO exception occurred during fetching of all file names from bucket {bucketName}, details: {ex}"),
                () => { doneFetchingFileNames.SetResult(new object()); });
            await doneFetchingFileNames.Task;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error occured when trying to read bucket {bucketName} contents, details: {ex}");
        }
        return allFileNames;
    }
}