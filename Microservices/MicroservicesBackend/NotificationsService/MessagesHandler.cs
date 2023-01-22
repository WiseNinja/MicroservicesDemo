using Connectivity;
using Microsoft.AspNetCore.SignalR;
using NotificationsService.Hubs;

namespace NotificationsService;

public class MessagesHandler
{
    private readonly ISubscriber _subscriber;
    private readonly IHubContext<MapEntitiesHub> _hubContext;
    private readonly ILogger<MessagesHandler> _logger;

    public MessagesHandler(ISubscriber subscriber, IHubContext<MapEntitiesHub> hubContext, ILogger<MessagesHandler> logger)
    {
        _subscriber = subscriber;
        _hubContext = hubContext;
        _logger = logger;
    }

    public async Task SubscribeAsync()
    {
        await _subscriber.SubscribeAsync(HandleMessage);
        _logger.Log(LogLevel.Information, "Subscribed to message broker");
    }

    private async void HandleMessage(string message)
    {
        throw new ApplicationException("test3");
        await _hubContext.Clients.All.SendAsync("MapPointAdded", message);
        _logger.Log(LogLevel.Information, "Sent Map Point Added notification to clients");
    }
}