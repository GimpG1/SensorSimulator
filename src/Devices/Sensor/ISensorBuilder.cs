using Configuration.Struct.Receiver;
using Configuration.Struct.Sensor;

namespace Devices.Sensor;

public interface ISensorBuilder : IBuilder
{
    ISensorBuilder Create(int id);
    ISensorBuilder SetType(SensorType type);
    ISensorBuilder SetRange(KeyValuePair<int, int> range);
    ISensorBuilder SetEncoderType(string encoder);
    ISensorBuilder SetFrequency(int frequency);
    ISensorBuilder AddSubscriber(ISubscriber subscriber);
    Sensor Build();
}