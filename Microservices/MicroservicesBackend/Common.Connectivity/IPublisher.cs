namespace Connectivity;

public interface IPublisher
{ 
    Task PublishAsync(Message message);
}
