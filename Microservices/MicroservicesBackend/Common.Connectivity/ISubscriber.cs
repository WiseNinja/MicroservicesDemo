namespace Connectivity
{
    public interface ISubscriber
    {
        Task SubscribeAsync(Action<string> handleMessage);
    }
}
