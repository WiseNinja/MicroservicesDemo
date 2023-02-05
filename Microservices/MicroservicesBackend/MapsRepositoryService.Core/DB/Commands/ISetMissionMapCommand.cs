namespace MapsRepositoryService.Core.DB.Commands;

public interface ISetMissionMapCommand
{
    Task SetMainMissionMapAsync(string? mapName);
}