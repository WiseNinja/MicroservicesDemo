using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MapEntitiesService.Core.Interfaces;
using MapEntitiesService.Core.Messages;

namespace MapEntitiesService.Infrastructure.MessageServiceClients
{
    public class RabbitMqClient : IMessageServiceClient
    {
        public Task SendMessage(BaseMessage message)
        {
            throw new NotImplementedException();
        }
    }
}
