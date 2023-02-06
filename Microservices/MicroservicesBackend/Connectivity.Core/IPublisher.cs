namespace Connectivity.Core;

public interface IPublisher
{ 
    Task<bool> PublishAsync(string message);
}
