using Connectivity;
using Microsoft.AspNetCore.SignalR;
using NotificationsService.Hubs;

namespace NotificationsService;

public class MessagesHandler
{
    private readonly ISubscriber _subscriber;
    private readonly IHubContext<MapEntitiesHub> _hubContext;

    public MessagesHandler(ISubscriber subscriber, IHubContext<MapEntitiesHub> hubContext)
    {
        _subscriber = subscriber;
        _hubContext = hubContext;
    }

    public async Task SubscribeAsync()
    {
        await _subscriber.SubscribeAsync(HandleMessage);
    }

    private async void HandleMessage(string message)
    {
        await _hubContext.Clients.All.SendAsync("MapPointAdded", message);
    }
}