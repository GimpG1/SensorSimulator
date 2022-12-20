using System.Text.Json.Serialization;

namespace Configuration.Struct.Sensor;

public sealed class SensorJson: ISensorData
{
    [JsonPropertyName("ID")]
    public int Id { get; set; }
    [JsonPropertyName("Type")]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public SensorType Type { get; set; }
    public int MinValue { get; set; }
    public int MaxValue { get; set; }
    public string? EncoderType { get; set; }
    public int Frequency { get; set; }
}