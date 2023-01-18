using MapEntitiesService.Core.Messages;

namespace MapEntitiesService.Core.Interfaces
{
    public interface IMessageServiceClient
    {
        public Task SendMessage(BaseMessage message);
    }
}
