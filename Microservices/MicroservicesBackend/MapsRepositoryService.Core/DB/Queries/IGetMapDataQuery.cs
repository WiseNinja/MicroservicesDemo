namespace MapsRepositoryService.Core.DB.Queries;

public interface IGetMapDataQuery
{
    Task<string> GetMapDataByNameAsync(string mapName);
}