using MapEntitiesService.Core.DTOs;

namespace MapEntitiesService.Core.Messages
{
    public class MapPointAddedMessage : BaseMessage
    {
        public MapPointDto MapPoint { get; set; }
    }
}
