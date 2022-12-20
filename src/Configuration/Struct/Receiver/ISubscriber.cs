namespace Configuration.Struct.Receiver;

public interface ISubscriber
{
    string? Message { get; }
    void Notify(IPublisherMessage message);
}