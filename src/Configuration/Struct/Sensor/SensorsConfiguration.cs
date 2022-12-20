namespace Configuration.Struct.Sensor;

public class SensorsConfiguration : IConfiguration
{
    public List<SensorJson> Sensors { get; set; } = new ();
}