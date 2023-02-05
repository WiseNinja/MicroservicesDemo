namespace MapsRepositoryService.Core.DB.Commands;

public interface IDeleteMapCommand
{
    Task<bool> DeleteMapAsync(string mapName);
}