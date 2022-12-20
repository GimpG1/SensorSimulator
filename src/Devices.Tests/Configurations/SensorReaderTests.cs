using System.Text.Json;
using Configuration.Struct.Sensor;
using TestHelper;

namespace Sensor.Tests.Configurations;

public class SensorReaderTests
{
    [Test]
    public async Task When_ReadNonExistingFileAsync_Expect_TaskCompleted()
    {
        // Arrange
        var reader = new SensorReader();
        var expected = new SensorsConfiguration();

        // Act
        await reader.ReadAsync();

        // Assert
        Assert.That(reader.Config.Sensors, Has.Count.EqualTo(expected.Sensors.Count));
    }

    [Test]
    public async Task When_ReadSucceed_ExpectEmptyConfig()
    {
        // Arrange
        var readerMock = new Mock<SensorReader>();
        var fileInfoHelper = new FileInfoHelper();
        var configHelper = new ConfigurationHelper();
        var sensorConfigFile = Path.Combine(fileInfoHelper.GetAssemblyLocation(typeof(SensorReaderTests)) ?? string.Empty,
            "Config\\sensorConfig.json");
        readerMock.Setup(file => file.ConfigName).Returns(sensorConfigFile);
        var exampleConfig = JsonSerializer.Deserialize<SensorsConfiguration>(configHelper.ExampleSensorConfigContent());

        // Act
        await readerMock.Object.ReadAsync();
        await Task.Delay(200);

        // Assert
        Assert.That(readerMock.Object.Config.Sensors, Is.Not.Empty);
        Assert.That(readerMock.Object.Config, Is.Not.Null);
        Assert.That(exampleConfig, Is.Not.Null);
        Assert.That(exampleConfig?.Sensors.Count, Is.EqualTo(readerMock.Object.Config.Sensors.Count));
    }
}