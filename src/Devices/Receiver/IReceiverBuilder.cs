namespace Devices.Receiver;

public interface IReceiverBuilder : IBuilder
{
    IReceiverBuilder Create(int id);
    IReceiverBuilder SetActive(bool activate);
    IReceiverBuilder SetListedId(int id);
    Receiver Build();
}