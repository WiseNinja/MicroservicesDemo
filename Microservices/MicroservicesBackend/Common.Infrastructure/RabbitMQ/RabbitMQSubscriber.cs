using Common.Connectivity;
using EasyNetQ;

namespace Common.Infrastructure.RabbitMQ;

public class RabbitMqSubscriber : ISubscriber
{
    private readonly IBus _bus;
    public RabbitMqSubscriber()
    {
        _bus = RabbitHutch.CreateBus("host=rabbitmq");
    }
    public async Task SubscribeAsync()
    {
        await _bus.PubSub.SubscribeAsync<Message>(
            "my_subscription_id", msg => Console.WriteLine(msg.MessageType)
        );
    }
}