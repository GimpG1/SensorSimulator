using Configuration.Struct.Receiver;

namespace Configuration.Struct.Sensor;

public interface IMessage
{
    List<ISubscriber> Subscribers { get; }
    void Publish();
}