using Connectivity.Core;
using EasyNetQ;
using Microsoft.Extensions.Logging;

namespace Infrastructure.RabbitMQ;

internal class RabbitMqSubscriber : ISubscriber
{
    private readonly ILogger<RabbitMqSubscriber> _logger;
    private readonly IBus _bus;
    public RabbitMqSubscriber(ILogger<RabbitMqSubscriber> logger)
    {
        _logger = logger;
        _bus = RabbitHutch.CreateBus("host=rabbitmq");
    }
    public async Task<bool> SubscribeAsync(Action<string> handleMessage)
    {
        try
        {
            await _bus.PubSub.SubscribeAsync<string>(
                "my_subscription_id", handleMessage);
        }
        catch (Exception ex)
        {
            _logger.LogError($"An error occurred when trying to Subscribe to RabbitMQ publisher, details {ex}");
            return false;
        }

        return true;
    }

}