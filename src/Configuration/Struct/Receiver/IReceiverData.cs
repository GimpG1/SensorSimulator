namespace Configuration.Struct.Receiver;

public interface IReceiverData : IActive
{
    public int Id { get; set; }
    public int ListenedSensorId { get; set; }
}