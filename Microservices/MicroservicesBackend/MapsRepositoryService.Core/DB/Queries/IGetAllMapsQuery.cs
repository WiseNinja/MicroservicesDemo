namespace MapsRepositoryService.Core.DB.Queries;

public interface IGetAllMapsQuery
{
    Task<List<string>?> GetAllMapNamesAsync();
}