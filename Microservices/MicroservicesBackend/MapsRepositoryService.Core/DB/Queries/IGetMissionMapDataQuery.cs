namespace MapsRepositoryService.Core.DB.Queries;

public interface IGetMissionMapDataQuery
{
    Task<string> GetMissionMapDataByNameAsync(string mapName);
}