using Configuration.Struct.Sensor;

namespace Configuration.Struct.Receiver;

public class SensorMessage : IPublisherMessage
{
    public ISensorData Sensor { get; set; } = null!;
    public int Value { get; set; }
}