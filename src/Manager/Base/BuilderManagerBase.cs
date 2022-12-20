using System.ComponentModel;
using System.Runtime.CompilerServices;
using Configuration.Struct;
using Configuration.Struct.Receiver;
using Configuration.Struct.Sensor;
using Devices;
using Devices.Receiver;
using Devices.Sensor;

namespace Manager.Base;

public abstract class BuilderManagerBase : INotifyPropertyChanged
{
    private bool _isReady = false;
    private IReceiverBuilder? _receiveBuilder;
    private ISensorBuilder? _sensorBuilder;
    private List<IReceiverData> _receiversList = new();

    public BuilderManagerBase()
    {
        PropertyChanged += CheckIfIsReady;
    }

    private void CheckIfIsReady(object? sender, PropertyChangedEventArgs e)
    {
        _isReady = _receiveBuilder != null &&
                   _sensorBuilder != null &&
                   _receiversList.Any();
    }

    public virtual void Append(IBuilder builder)
    {
        if (builder is IReceiverBuilder receiverBuilder)
        {
            _receiveBuilder = receiverBuilder;
        }

        if (builder is ISensorBuilder sensorBuilder)
        {
            _sensorBuilder = sensorBuilder;
        }

        OnPropertyChanged();
    }

    public virtual void Resolve(IConfiguration configurations)
    {
        if (configurations is ReceiversConfiguration receiversConfig)
        {
            ResolveReceivers(receiversConfig);
        }

        if (configurations is SensorsConfiguration sensorsConfig)
        {
            ResolveSensors(sensorsConfig);
        }
        OnPropertyChanged();
    }

    private void ResolveSensors(SensorsConfiguration sensorsConfig)
    {
        foreach (var sensorJson in sensorsConfig.Sensors)
        {
            _sensorBuilder?.Create(sensorJson.Id)
                .SetType(sensorJson.Type)
                .SetRange(new KeyValuePair<int, int>(sensorJson.MinValue, sensorJson.MaxValue))
                .SetFrequency(sensorJson.Frequency)
                .SetEncoderType(sensorJson.EncoderType!);

            AppendProperReceiver(sensorJson);

            _sensorBuilder?.Build().SetActive(true);
        }
    }

    private void AppendProperReceiver(SensorJson sensorJson)
    {
        foreach (var receiver in _receiversList.Where(item => item.ListenedSensorId == sensorJson.Id))
        {
            _sensorBuilder?.AddSubscriber((ISubscriber)receiver);
        }
    }

    private void ResolveReceivers(ReceiversConfiguration receiversConfig)
    {
        if (_receiveBuilder != null)
        {
            foreach (var receiverJson in receiversConfig.Receivers)
            {
                _receiversList.Add(_receiveBuilder.Create(receiverJson.Id)
                    .SetActive(receiverJson.IsActive)
                    .SetListedId(receiverJson.ListenedSensorId)
                    .Build());
            }
        }
    }

    public bool IsReadyToUse => _isReady;

    public List<IReceiverData> GetReceivers()
    {
        if (_receiversList.Any())
        {
            return _receiversList;
        }

        return Enumerable.Empty<IReceiverData>().ToList();
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}