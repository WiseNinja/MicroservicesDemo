using Common.Connectivity;
using Common.Connectivity.Enums;
using Common.Logging;
using MapEntitiesService.Core.DTOs;
using MapEntitiesService.Core.Interfaces;
using Newtonsoft.Json;

namespace MapEntitiesService.Core.Features.MapPoints;

public class MapPointsService : IMapPointsService
{
    private readonly ILoggingService _loggingService;
    private readonly IPublisher _publisher;

    public MapPointsService(ILoggingService loggingService, IPublisher publisher)
    {
        _loggingService = loggingService;
        _publisher = publisher;
    }
    public async Task AddNewMapPointAsync(MapPointDto mapPointDto)
    {
        Message message = new Message
        {
            MessageId = Guid.NewGuid(),
            MessageType = MessageType.AddMapPoint,
            Payload = JsonConvert.SerializeObject(mapPointDto)
        };
        await _publisher.PublishAsync(message);
        await _loggingService.LogInformationAsync("New Map Endpoint added successfully");
    }
}