namespace MapsRepositoryService.Core.DB.Commands;

public interface IDeleteMapCommand
{
    Task DeleteMapAsync(string mapName);
}