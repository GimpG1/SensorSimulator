using Configuration.Struct.Receiver;

namespace Configuration.Struct.Sensor;

public interface ISensorDetails
{
    public int Hertz { get;}
    List<ISubscriber> Subscribers { get; }
}