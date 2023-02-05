using Connectivity;
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
    public async Task AddNewMapPointAsync(MapPointDto mapPoint)
    {
        string newMapPointToBeAdded = JsonConvert.SerializeObject(mapPoint);
        await _publisher.PublishAsync(newMapPointToBeAdded);
        _logger.Log(LogLevel.Information, "New Map Point creation started");
    }
}