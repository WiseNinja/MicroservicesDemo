using MapsRepositoryService.Core.DTOs;

namespace MapsRepositoryService.Core.DB.Commands;

public interface IInsertMapCommand
{
    Task InsertMapAsync(MapDto mapDto);
}