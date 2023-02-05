using Connectivity.Core;
using MapEntitiesService.Core.DTOs;
using MapEntitiesService.Core.Interfaces;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace MapEntitiesService.Core.Features.MapPoints;

public class MapPointsService : IMapPointsService
{
    private readonly IPublisher _publisher;
    private readonly ILogger<MapPointsService> _logger;

    public MapPointsService(IPublisher publisher, ILogger<MapPointsService> logger)
    {
        _publisher = publisher;
        _logger = logger;
    }
    public async Task<bool> AddNewMapPointAsync(MapPointDto mapPoint)
    {
        var newMapPointToBeAdded = string.Empty; 
        try
        {
             newMapPointToBeAdded = JsonConvert.SerializeObject(mapPoint);
        }
        catch (Exception ex)
        {
            _logger.LogError($"An error occured when trying to serialize the Map point entity to be added, details: {ex}");
        }
        
        var pointAddedPublishWasSuccessful = await _publisher.PublishAsync(newMapPointToBeAdded);
        if (pointAddedPublishWasSuccessful && !string.IsNullOrEmpty(newMapPointToBeAdded))
        {
            return true;
        }

        return false;
    }
}