namespace Connectivity.Core;

public interface ISubscriber
{
    Task<bool> SubscribeAsync(Action<string> handleMessage);
}