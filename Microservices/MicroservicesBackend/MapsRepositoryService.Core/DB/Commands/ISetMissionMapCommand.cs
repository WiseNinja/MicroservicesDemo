namespace MapsRepositoryService.Core.DB.Commands;

public interface ISetMissionMapCommand
{
    Task<bool> SetMainMissionMapAsync(string? mapName);
}