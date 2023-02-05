using MapsRepositoryService.Core.DTOs;

namespace MapsRepositoryService.Core.DB.Commands;

public interface IInsertMapCommand
{
    Task<bool> InsertMapAsync(MapDto mapDto);
}