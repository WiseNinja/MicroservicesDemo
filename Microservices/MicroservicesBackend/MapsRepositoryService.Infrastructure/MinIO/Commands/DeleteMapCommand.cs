using MapsRepositoryService.Core.DB.Commands;
using Microsoft.Extensions.Logging;
using Minio;

namespace MapsRepositoryService.Infrastructure.MinIO.Commands;

internal class DeleteMapCommand : IDeleteMapCommand
{
    private readonly MinioClient _minioClient;
    private readonly ILogger<DeleteMapCommand> _logger;

    public DeleteMapCommand(MinioClient minioClient, ILogger<DeleteMapCommand> logger)
    {
        _minioClient = minioClient;
        _logger = logger;
    }

    public async Task<bool> DeleteMapAsync(string mapName)
    {
        try
        {
            var removeObjectArgs = new RemoveObjectArgs()
                .WithBucket("maps-bucket")
                .WithObject(mapName);

            await _minioClient.RemoveObjectAsync(removeObjectArgs);
        }
        catch (Exception ex)
        {
            _logger.LogError($"An error occurred when trying to delete Map {mapName} from storage, details {ex}");
            return false;
        }

        return true;
    }
}