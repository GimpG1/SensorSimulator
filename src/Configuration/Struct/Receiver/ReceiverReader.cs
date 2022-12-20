namespace Configuration.Struct.Receiver;

public class ReceiverReader : ConfigurationBase, IRead
{
    public ReceiversConfiguration Config => (ReceiversConfiguration)Configuration! ?? new ReceiversConfiguration();

    public override string ConfigName => "receiversConfiguration.json";

    public ReceiverReader()
    {
        SetReadMethod(ReadConfig);
    }

    public async Task ReadConfig()
    {
        await using var readStream = File.OpenRead(ConfigPath);
        base.Configuration = await JsonSerializer.DeserializeAsync<ReceiversConfiguration>(readStream);
    }
}