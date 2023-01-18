using MapEntitiesService.Core.DTOs;
using MapEntitiesService.Core.Interfaces;
using MapEntitiesService.Core.Messages;

namespace MapEntitiesService.Core.Features.MapPoints
{
    public class MapPointsService : IMapPointsService
    {
        private readonly ILoggingService _loggingService;
        private readonly IMessageServiceClient _messageServiceClient;

        public MapPointsService(ILoggingService loggingService, IMessageServiceClient messageServiceClient)
        {
            _loggingService = loggingService;
            _messageServiceClient = messageServiceClient;
        }
        public async Task AddNewMapPoint(MapPointDto mapPointDto)
        {
            MapPointAddedMessage mapPointAddedMessage = new MapPointAddedMessage
            {
                MessageId = Guid.NewGuid().ToString(),
                MapPoint = mapPointDto
            };
            
            await _messageServiceClient.SendMessage(mapPointAddedMessage);
            await _loggingService.Log("New Map Endpoint added successfully");
        }
    }
}
