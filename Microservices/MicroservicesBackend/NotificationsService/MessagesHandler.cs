using Common.Connectivity;
using Connectivity;

namespace NotificationsService;

public class MessagesHandler
{
    private readonly ISubscriber _subscriber;

    public MessagesHandler(ISubscriber subscriber)
    { 
        _subscriber = subscriber;
    }

    public async Task HandleMessagesAsync()
    {
        await _subscriber.SubscribeAsync(HandleMessage);
    }

    private void HandleMessage(string obj)
    {
       
    }
}