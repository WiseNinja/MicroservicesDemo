namespace MapsRepositoryService.Core.DB.Queries;

public interface IGetMissionMapNameQuery
{
    Task<string> GetMissionMapNameAsync();
}