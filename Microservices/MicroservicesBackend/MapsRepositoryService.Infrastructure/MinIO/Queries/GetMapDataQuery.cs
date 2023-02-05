using MapsRepositoryService.Core.DB.Queries;
using MapsRepositoryService.Infrastructure.MinIO.Helpers;

namespace MapsRepositoryService.Infrastructure.MinIO.Queries;

public class GetMapDataQuery : IGetMapDataQuery
{
    private readonly FileOperationsHelper _fileOperationsHelper;

    public GetMapDataQuery(FileOperationsHelper fileOperationsHelper)
    {
        _fileOperationsHelper = fileOperationsHelper;
    }

    public async Task<string> GetMapDataByNameAsync(string mapName)
    {
        return await _fileOperationsHelper.GetFileDataAsBase64(mapName,"maps-bucket");
    }
}