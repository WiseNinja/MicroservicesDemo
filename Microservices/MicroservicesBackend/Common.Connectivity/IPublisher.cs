namespace Common.Connectivity;

public interface IPublisher
{ 
    Task PublishAsync(Message message);
}
