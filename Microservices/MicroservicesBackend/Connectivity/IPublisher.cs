namespace Connectivity;

public interface IPublisher
{ 
    Task PublishAsync(string message);
}
