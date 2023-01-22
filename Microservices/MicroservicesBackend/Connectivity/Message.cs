using Common.Connectivity.Enums;

namespace Connectivity;

public class Message
{
    public Guid MessageId { get; set; }
    public MessageType MessageType { get; set; }
    public string? Payload { get; set; }
}