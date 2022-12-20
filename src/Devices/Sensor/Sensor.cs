using System.Timers;
using Configuration.Struct.Receiver;
using Configuration.Struct.Sensor;

namespace Devices.Sensor;

public sealed class Sensor : ISensorData, ISensorDetails, IPublisher, IMessage, IActive, IDisposable
{
    private static readonly System.Timers.Timer _timer = new();
    public int Id { get; internal set; }
    public SensorType Type { get; internal set; }
    public int MinValue { get; internal set; }
    public int MaxValue { get; internal set; }
    public string? EncoderType { get; internal set; }
    public int Frequency { get; internal set; }
    public int Hertz => (int)Math.Round(_timer.Interval);
    public bool IsActive { get; set; }
    public List<ISubscriber> Subscribers { get; internal set; } = new();

    public Sensor()
    {
        _timer.Elapsed += OnFrequencyCall;
    }

    public void OnFrequencyCall(object? sender, ElapsedEventArgs e)
    {
        var rand = new Random().Next(MinValue, MaxValue);
        for (int i = 0; i < Subscribers.Count; i++)
        {
            this.Subscribers[i].Notify(new SensorMessage()
            {
                Sensor = this,
                Value = rand
            });
        }
    }

    public void SetInterval()
    {
        _timer.Interval = 1000.0 / this.Frequency;
    }

    public void SetActive(bool active)
    {
        if (this.Subscribers.Any())
        {
            IsActive = active;
        }
        else
        {
            throw new NullReferenceException("Please add Subscriber, and try to active again");
        }

        if (!active)
        {
            IsActive = active;
        }

        Publish();
    }

    public void Publish()
    {
        if (IsActive)
        {
            _timer.Start();
            return;
        }

        _timer.Stop();
    }

    public void Dispose()
    {
        SetActive(false);
    }
}