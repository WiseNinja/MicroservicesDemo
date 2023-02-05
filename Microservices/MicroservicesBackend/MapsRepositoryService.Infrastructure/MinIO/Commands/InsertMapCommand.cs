using MapsRepositoryService.Core.DB.Commands;
using MapsRepositoryService.Core.DTOs;
using MapsRepositoryService.Infrastructure.MinIO.Helpers;

namespace MapsRepositoryService.Infrastructure.MinIO.Commands;

public class InsertMapCommand : IInsertMapCommand
{
    private readonly FileOperationsHelper _fileOperationsHelper;

    public InsertMapCommand(FileOperationsHelper fileOperationsHelper)
    {
        _fileOperationsHelper = fileOperationsHelper;
    }
    public async Task InsertMapAsync(MapDto mapDto)
    {
        await _fileOperationsHelper.UploadFileAsync(mapDto.Name, mapDto.Data, "maps-bucket");
    }
}