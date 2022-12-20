using Configuration.Struct.Receiver;
using Configuration.Struct.Sensor;
using DeviceReceiver = Devices.Receiver.Receiver;

namespace Devices.Sensor;

public class SensorBuilder : ISensorBuilder
{
    private Sensor _sensor = new ();

    public ISensorBuilder Create(int id)
    {
        _sensor = new();
        this._sensor.Id = id;
        return this;
    }

    public ISensorBuilder SetType(SensorType type)
    {
        this._sensor.Type = type;
        return this;
    }

    public ISensorBuilder SetRange(KeyValuePair<int, int> range)
    {
        if (range.Key > range.Value)
        {
            this._sensor.MaxValue = range.Key;
            this._sensor.MinValue = range.Value;
        }
        else
        {
            this._sensor.MaxValue = range.Value;
            this._sensor.MinValue = range.Key;
        }

        return this;
    }

    public ISensorBuilder SetEncoderType(string encoder)
    {
        this._sensor.EncoderType = encoder;
        return this;
    }

    public ISensorBuilder SetFrequency(int frequency)
    {
        this._sensor.Frequency = frequency;
        return this;
    }

    public ISensorBuilder AddSubscriber(ISubscriber subscriber)
    {
        if (subscriber is DeviceReceiver receiver)
        {
            if (receiver.SetSensor(_sensor))
            {
                this._sensor.Subscribers.Add(subscriber);
            }
            else
            {
                throw new ArgumentException("Given Receiver is already in use by another sensor, please use another receiver and try again");
            }
        }

        return this;
    }
    public Sensor Build()
    {
        if (_sensor.Frequency != 0)
        {
            _sensor.SetInterval();
        }

        return _sensor;
    }
}