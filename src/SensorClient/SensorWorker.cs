using Configuration.Struct;
using Configuration.Struct.Receiver;
using Configuration.Struct.Sensor;
using Devices.Receiver;
using Devices.Sensor;
using Manager.Workers;

namespace SensorClient;

public class SensorWorker : IConfigurationReaderAsync 
{
    private SensorsConfiguration _configuration = null!;
    
    public async Task Read(ConfigurationBase reader)
    {
        try
        {
            if (reader is SensorReader sensorReader)
            {
                await sensorReader.ReadAsync();
                await Task.Delay(100);
                _configuration = sensorReader.Config;
            }
        }
        catch (ArgumentException e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task Resolve()
    {
        await Task.Run(() =>
        {
            var receiverBuilder = new ReceiverBuilder();
            var sensorBuilder = new SensorBuilder();
            foreach (var sensorJson in _configuration.Sensors)
            {
                var sensor = sensorBuilder?.Create(sensorJson.Id)
                    .SetType(sensorJson.Type)
                    .SetRange(new KeyValuePair<int, int>(sensorJson.MinValue, sensorJson.MaxValue))
                    .SetFrequency(sensorJson.Frequency)
                    .SetEncoderType(sensorJson.EncoderType!)
                    .AddSubscriber(receiverBuilder.Create(sensorJson.Id).SetActive(true).Build())
                    .Build();
            
                sensor?.SetActive(true);
            
                if (sensor != null) 
                    Sensors.Add(sensor);
            }
        });
    }

    public ISubscriber GetMessageToSend(ISensorData sensor)
    {
        return ((Sensor)sensor).Subscribers.First();
    }

    public List<ISensorData> Sensors { get; } = new();
}