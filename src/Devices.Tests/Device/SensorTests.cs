using Configuration.Struct.Sensor;
using Devices.Receiver;
using PSensor = Devices.Sensor;

namespace Sensor.Tests.Device;

public class SensorTests
{
    [Test]
    public void When_ObjectCreated_CheckFluentSets()
    {
        // Arrange
        var sensorDevice = new PSensor.SensorBuilder();

        // Act
        var sensor = sensorDevice.Create(1)
            .SetType(SensorType.Position)
            .SetRange(new KeyValuePair<int, int>(0, 100))
            .SetFrequency(2)
            .SetEncoderType("fixed")
            .Build();

        // Assert
        Assert.That(sensor.Id, Is.EqualTo(1));
        Assert.That(sensor.Type, Is.EqualTo(SensorType.Position));
        Assert.That(sensor.MinValue, Is.EqualTo(0));
        Assert.That(sensor.MaxValue, Is.EqualTo(100));
        Assert.That(sensor.Frequency, Is.EqualTo(2));
        Assert.That(sensor.EncoderType, Is.EqualTo("fixed"));
    }

    [Test]
    [TestCase(1, 1000)]
    [TestCase(2, 500)]
    [TestCase(10, 100)]
    public void When_Given_Frequency_Then_CheckIfEqualsExpected(int frequency, int expected)
    {
        // Arrange
        var sensorDevice = new PSensor.SensorBuilder();

        // Act
        var sensor = sensorDevice.Create(1)
            .SetType(SensorType.Position)
            .SetRange(new KeyValuePair<int, int>(0, 100))
            .SetFrequency(frequency)
            .SetEncoderType("fixed")
            .Build();

        // Assert
        Assert.That(sensor.Hertz, Is.EqualTo(expected));
    }

    [Test]
    public void When_ReceiverSubscribed_Then_CheckIfOnlyOneSensorIsAvailable()
    {
        // Arrange
        var receiverBuilder = new ReceiverBuilder();
        var receiverOne = receiverBuilder.Create(1)
            .SetActive(true)
            .Build();

        var sensorBuilder = new PSensor.SensorBuilder();
        var sensorOne = sensorBuilder.Create(1)
            .SetType(SensorType.Position)
            .SetRange(new KeyValuePair<int, int>(0, 100))
            .SetFrequency(2)
            .SetEncoderType("fixed")
            // Act
            .AddSubscriber(receiverOne)
            .Build();

        var receiverTwo = receiverBuilder.Create(2)
            .SetActive(true)
            .Build();

        // Assert
        Assert.That(sensorOne.Subscribers.Count, Is.EqualTo(1));
        var firstSubscriber = sensorOne.Subscribers.First();
        Assert.That(((Receiver)firstSubscriber).SensorData!.Id, Is.EqualTo(sensorOne.Id));

        // Fail receiverOne already in use
        Assert.Throws<ArgumentException>(
            () =>
            {
                _ = sensorBuilder.Create(2)
                    .SetType(SensorType.Position)
                    .SetRange(new KeyValuePair<int, int>(-10, 10))
                    .SetFrequency(2)
                    .SetEncoderType("fixed")
                    // Act
                    .AddSubscriber(receiverOne)
                    .AddSubscriber(receiverTwo)
                    .Build();
            });
    }

    [Test]
    [TestCase(true)]
    [TestCase(false)]
    public async Task When_SensorActivated_Then_ExpectMessage(bool activateReceiver)
    {
        // Arrange
        var receiverBuilder = new ReceiverBuilder();
        var receiver = receiverBuilder.Create(1)
            .SetActive(activateReceiver)
            .Build();

        var sensorBuilder = new PSensor.SensorBuilder();
        var sensor = sensorBuilder.Create(1)
            .SetType(SensorType.Position)
            .SetRange(new KeyValuePair<int, int>(0, 100))
            .SetFrequency(2)
            .SetEncoderType("fixed")
            .Build();

        // Act
        sensorBuilder.AddSubscriber(receiver);
        sensor.SetActive(true);
        await Task.Delay(700);
        sensor.SetActive(false);

        // Assert
        if (activateReceiver)
        {
            Assert.IsNotEmpty(receiver.Message);
        }
        else
        {
            Assert.IsEmpty(receiver.Message);
        }
    }
}