using Configuration.Ranges;
using Devices.Calculation;

namespace Sensor.Tests.Device;

public class ReceiverTests
{
    [Test]
    [TestCase(-10, 10, -8f, -5f, 8f, 5f)]
    [TestCase(-10000, 10000, -8000f, -5000f, 8000f, 5000f)]
    [TestCase(0, 255, 26f, 64f, 230f, 191f)]
    public void When_RangeIsSet_Then_ExpectCalculations(int min, int max, float alarmMin,
        float warningMin, float alarmMax, float warningMax)
    {
        var total = Math.Abs(min) + Math.Abs(max);
        var alarm = 0.1f * total;
        var warning = .25f * total;

        var aMin = (float)Math.Round((min + alarm), MidpointRounding.AwayFromZero);
        var wMin = (float)Math.Round((min + warning), MidpointRounding.AwayFromZero);

        var aMax = (float)Math.Round((max - alarm), MidpointRounding.AwayFromZero);
        var wMax = (float)Math.Round((max - warning), MidpointRounding.AwayFromZero);

        Assert.That(aMin, Is.EqualTo(alarmMin));
        Assert.That(aMax, Is.EqualTo(alarmMax));
        Assert.That(wMin, Is.EqualTo(warningMin));
        Assert.That(wMax, Is.EqualTo(warningMax));
    }
    [Test]
    [TestCase(-10, 10, 2f, State.Normal)]
    [TestCase(-10, 10, -9f, State.MinAlarm)]
    [TestCase(-10, 10, -7f, State.MinWarning)]
    [TestCase(0, 255, 236f, State.MaxAlarm)]
    [TestCase(0, 255, 200f, State.MaxWarning)]
    public void When_RangeControllerIsCalled_Then_ExpectCalculations(int min, int max, float expectedPoint,
        State expectedStatus)
    {
        // Arrange
        var rangeController = new RangesController()
                            .Setup(new KeyValuePair<int, int>(min, max));
        // Act
        var status = rangeController.CalculateStatus(expectedPoint);
        
        // Assert
        Assert.That(status, Is.EqualTo(expectedStatus));
    }
}