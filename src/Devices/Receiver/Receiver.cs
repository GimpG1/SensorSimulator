using System.ComponentModel;
using System.Runtime.CompilerServices;
using Configuration.Struct.Receiver;
using Configuration.Struct.Sensor;
using Devices.Calculation;

namespace Devices.Receiver;

public sealed class Receiver : IReceiverData, ISubscriber, INotifyPropertyChanged
{
    private string _message = string.Empty;
    private readonly RangesController _rangesController = new();
    public ISensorData? SensorData { get; private set; }

    public string? Message
    {
        get => _message;
        private set
        {
            if (string.IsNullOrEmpty(value)) return;

            _message = value;
            OnPropertyChanged();
        }
    }

    public bool SetSensor(ISensorData? sensor)
    {
        if (sensor == null || SensorData != null)
        {
            return false;
        }

        SensorData = sensor;
        _rangesController.Setup(new KeyValuePair<int, int>(sensor.MinValue, sensor.MaxValue));
        return true;
    }

    public void Notify(IPublisherMessage message)
    {
        if (!IsActive)
        {
            return;
        }

        var status = _rangesController.CalculateStatus(message.Value);
        this.Message = $"$FIX, [{message.Sensor.Id}], [{message.Sensor.Type}], [{message.Value}], [{status}]";
    }

    #region INotifyPropertyChanged

    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    #endregion

    #region IReceiverData

    public int Id { get; set; }

    public int ListenedSensorId { get; set; }
    public bool IsActive { get; set; }

    #endregion
}