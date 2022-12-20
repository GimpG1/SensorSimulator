namespace Devices.Receiver;

public class ReceiverBuilder : IReceiverBuilder
{
    private Receiver _receiver = new();

    public IReceiverBuilder Create(int id)
    {
        _receiver = new();
        _receiver.Id = id;
        return this;
    }

    public IReceiverBuilder SetActive(bool activate)
    {
        _receiver.IsActive = activate;
        return this;
    }

    public IReceiverBuilder SetListedId(int id)
    {
        _receiver.ListenedSensorId = id;
        return this;
    }

    public Receiver Build()
    {
        return _receiver;
    }
}