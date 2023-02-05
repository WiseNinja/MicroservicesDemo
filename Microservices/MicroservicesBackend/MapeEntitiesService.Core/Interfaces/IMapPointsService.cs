using MapEntitiesService.Core.DTOs;

namespace MapEntitiesService.Core.Interfaces;

public interface IMapPointsService
{
    Task<bool> AddNewMapPointAsync(MapPointDto mapPoint);
}