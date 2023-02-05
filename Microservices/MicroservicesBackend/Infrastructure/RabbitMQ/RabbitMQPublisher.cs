using Connectivity;
using EasyNetQ;

namespace Infrastructure.RabbitMQ;

public class RabbitMqPublisher : IPublisher
{
    private readonly IBus _bus;

    public RabbitMqPublisher()
    {
        _bus = RabbitHutch.CreateBus("host=rabbitmq");
    }

    public async Task PublishAsync(string message)
    {
        await _bus.PubSub.PublishAsync(message);
    }
}