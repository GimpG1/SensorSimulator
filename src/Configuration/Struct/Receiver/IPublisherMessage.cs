using Configuration.Struct.Sensor;

namespace Configuration.Struct.Receiver;

public interface IPublisherMessage
{
    ISensorData Sensor { get; set; }
    int Value { get; set; }
}