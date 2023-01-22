using Common.Connectivity.Enums;
using Connectivity;
using MapEntitiesService.Core.DTOs;
using MapEntitiesService.Core.Interfaces;
using Newtonsoft.Json;

namespace MapEntitiesService.Core.Features.MapPoints;

public class MapPointsService : IMapPointsService
{
    private readonly IPublisher _publisher;

    public MapPointsService(IPublisher publisher)
    {
        //_logger = logger;
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
        //_logger.Log(LogLevel.Information, "New Map Point creation started");
    }
}