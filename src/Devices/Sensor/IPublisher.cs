using System.Timers;

namespace Devices.Sensor;

public interface IPublisher
{
    void OnFrequencyCall(object? sender, ElapsedEventArgs e);
}