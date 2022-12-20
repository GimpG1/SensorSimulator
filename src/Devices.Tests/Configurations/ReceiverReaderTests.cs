using System.Text.Json;
using Configuration.Struct.Receiver;
using TestHelper;

namespace Sensor.Tests.Configurations;

public class ReceiverReaderTests
{
    [Test]
    public async Task When_FailToReadReceiverConfig_Then_DoNotAllowToUseSimulation()
    {
        // Arrange
        var reader = new ReceiverReader();
        var expected = new ReceiversConfiguration();

        // Act
        await reader.ReadAsync();

        // Assert
        Assert.That(reader.Config.Receivers.Count, Is.EqualTo(expected.Receivers.Count));
    }
    
    [Test]
    public async Task When_ReadSucceed_ExpectEmptyConfig()
    {
        // Arrange
        var readerMock = new Mock<ReceiverReader>();
        var fileInfoHelper = new FileInfoHelper();
        var configHelper = new ConfigurationHelper();
        var receiverFileExample = Path.Combine(fileInfoHelper.GetAssemblyLocation(typeof(ReceiverReaderTests)) ?? string.Empty,
            "Config\\receiversConfiguration.json");
        readerMock.Setup(file => file.ConfigName).Returns(receiverFileExample);
        var exampleConfig = JsonSerializer.Deserialize<ReceiversConfiguration>(configHelper.ExampleReceiverConfig());

        // Act
        await readerMock.Object.ReadAsync();
        await Task.Delay(200);

        // Assert
        Assert.That(readerMock.Object.Config.Receivers, Is.Not.Empty);
        Assert.That(readerMock.Object.Config, Is.Not.Null);
        Assert.That(exampleConfig, Is.Not.Null);
        Assert.That(exampleConfig?.Receivers.Count, Is.EqualTo(readerMock.Object.Config.Receivers.Count));
    }
}