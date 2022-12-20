namespace Configuration.Struct.Receiver;

public sealed class ReceiverJson : IReceiverData
{
    public int Id { get; set; }
    public bool IsActive { get; set; }
    public int ListenedSensorId { get; set; }
}