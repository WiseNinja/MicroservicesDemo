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
        try
        {
            await _subscriber.SubscribeAsync(HandleMessage);
            _logger.LogInformation("Subscribed to message broker");
        }
        catch (Exception ex)
        {
            _logger.LogError($"An error occurred during subscription to the message broker, details: {ex}");
        }
    }

    private async void HandleMessage(string message)
    {
        try
        {
            await _hubContext.Clients.All.SendAsync("MapPointAdded", message);
            await _hubContext.Clients.All.SendAsync("MissionMapSet", message);
            _logger.LogInformation("Sent all notifications to clients");
        }
        catch (Exception ex)
        {
            _logger.LogError($"An error occurred during handling a message from the message broker, details: {ex}");
        }
    }
}