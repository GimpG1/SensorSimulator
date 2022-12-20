namespace Configuration.Struct.Sensor;

public class SensorReader : ConfigurationBase, IRead
{
    public SensorsConfiguration Config => (SensorsConfiguration)Configuration! ?? new SensorsConfiguration();
    public override string ConfigName => "sensorConfig.json";

    public SensorReader()
    {
        SetReadMethod(ReadConfig);
    }

    public async Task ReadConfig()
    {
        await using var readStream = File.OpenRead(ConfigPath);
        base.Configuration = await JsonSerializer.DeserializeAsync<SensorsConfiguration>(readStream);
    }
}