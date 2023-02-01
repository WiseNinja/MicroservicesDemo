using MapsRepositoryService.Core.DB.Commands;
using MapsRepositoryService.Infrastructure.MinIO.Helpers;

namespace MapsRepositoryService.Infrastructure.MinIO.Commands;

public class DeleteMapCommand : IDeleteMapCommand
{
    private readonly FileOperationsHelper _fileOperationsHelper;

    public DeleteMapCommand(FileOperationsHelper fileOperationsHelper)
    {
        _fileOperationsHelper = fileOperationsHelper;
    }
    public async Task DeleteMapAsync(string mapName)
    {
        await _fileOperationsHelper.DeleteFileAsync(mapName, "maps-bucket");
    }
}