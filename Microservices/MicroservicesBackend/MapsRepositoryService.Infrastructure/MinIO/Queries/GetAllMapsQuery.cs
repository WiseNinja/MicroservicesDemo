using MapsRepositoryService.Core.DB.Queries;
using MapsRepositoryService.Infrastructure.MinIO.Helpers;

namespace MapsRepositoryService.Infrastructure.MinIO.Queries;

internal class GetAllMapsQuery : IGetAllMapsQuery
{
    private readonly FileOperationsHelper _fileOperationsHelper;

    public GetAllMapsQuery(FileOperationsHelper fileOperationsHelper)
    {
        _fileOperationsHelper = fileOperationsHelper;
    }
    public async Task<List<string>> GetAllMapNamesAsync()
    {
        return await _fileOperationsHelper.GetAllFileNamesFromBucketAsync("maps-bucket");
    }
}