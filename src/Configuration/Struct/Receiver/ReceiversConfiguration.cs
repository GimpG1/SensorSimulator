namespace Configuration.Struct.Receiver;

public class ReceiversConfiguration : IConfiguration
{
    public List<ReceiverJson> Receivers { get; set; } = new ();
}