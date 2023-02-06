using Connectivity.Core;
using EasyNetQ;
using Microsoft.Extensions.Logging;

namespace Infrastructure.RabbitMQ;

internal class RabbitMqPublisher : IPublisher
{
    private readonly ILogger<RabbitMqPublisher> _logger;
    private readonly IBus _bus;

    public RabbitMqPublisher(ILogger<RabbitMqPublisher> logger)
    {
        _logger = logger;
        _bus = RabbitHutch.CreateBus("host=rabbitmq");
    }

    public async Task<bool> PublishAsync(string message)
    {
        try
        {
            await _bus.PubSub.PublishAsync(message);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Exception occurred when trying to publish a message to clients via RabbitMQ, details: {ex}");
            return false;
        }

        return true;
    }
}