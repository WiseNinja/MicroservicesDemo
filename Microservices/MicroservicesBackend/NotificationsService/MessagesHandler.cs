using Common.Connectivity;

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
        await _subscriber.SubscribeAsync();
    }
}