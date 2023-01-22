using Connectivity;
using EasyNetQ;

namespace Infrastructure.RabbitMQ;

public class RabbitMqSubscriber : ISubscriber
{
    private readonly IBus _bus;
    public RabbitMqSubscriber()
    {
        _bus = RabbitHutch.CreateBus("host=rabbitmq");
    }
    public async Task SubscribeAsync(Action<string> handleMessage)
    {
        await _bus.PubSub.SubscribeAsync<Message>(
            "my_subscription_id", msg => handleMessage(msg.Payload));
    }

}