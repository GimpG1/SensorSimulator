namespace Configuration.Struct.Sensor;

public interface ISensorData
{
    public int Id { get; }
    public SensorType Type { get; }
    public int MinValue { get; }
    public int MaxValue { get; }
    public string? EncoderType { get; }
    public int Frequency { get; }
}