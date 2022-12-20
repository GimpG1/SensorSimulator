using System.Text.Json;
using Configuration.Struct.Receiver;
using Configuration.Struct.Sensor;
using Devices.Receiver;
using Devices.Sensor;
using TestHelper;

namespace Manager.Tests;

public class ReadAndBuildTests
{
    [Test]
    public void When_ReadConfigSucceed_Then_BuildSensorsAndReceivers()
    {
        // Arrange
        var manager = new BuilderManager();
        var sensorBuilder = new SensorBuilder();
        var receiverBuilder = new ReceiverBuilder();
        var configHelper = new ConfigurationHelper();
        var mReceiverConfig = JsonSerializer.Deserialize<ReceiversConfiguration>(configHelper.ExampleReceiverConfig());
        var mSensorConfig = JsonSerializer.Deserialize<SensorsConfiguration>(configHelper.ExampleSensorConfigContent());
        
        // Act
        manager.Append(sensorBuilder);
        manager.Append(receiverBuilder);
        manager.Resolve(mReceiverConfig!);
        manager.Resolve(mSensorConfig!);
        
        // Assert
        Assert.IsTrue(manager.IsReadyToUse);
        Assert.IsNotNull(manager.GetReceivers());
    }
}