namespace Manager.Tests;

public class AnalyzeAndPrintTests
{
    [Test]
    [TestCase("$FIX [1], [Depth], [2], [Normal]", ConsoleColor.Green)]
    [TestCase("$FIX [1], [Depth], [-1], [MinWarning]", ConsoleColor.Yellow)]
    [TestCase("$FIX [1], [Depth], [-5], [MinAlarm]", ConsoleColor.Red)]
    [TestCase("$FIX [1], [Depth], [5], [MaxWarning]", ConsoleColor.Yellow)]
    [TestCase("$FIX [1], [Depth], [7], [MaxAlarm]", ConsoleColor.Red)]
    public void When_PrintIsRequired_Then_CheckTheMessageColor(string message, ConsoleColor expectedColor)
    {
        // Arrange
        var analyzerManager = new AnalyzeManager();
        
        // Act
        var color = analyzerManager.AnalyzeMessage(message);
        
        // Assert
        Assert.That(color, Is.EqualTo(expectedColor));
    }
    
    [Test]
    [TestCase("$FIX [1], [Depth], [2], [Normal]", "I'm ok")]
    [TestCase("$FIX [1], [Depth], [-1], [MinWarning]", "I'm getting closer so fast - sensor value: -1")]
    [TestCase("$FIX [1], [Depth], [-5], [MinAlarm]", "Too Close!")]
    [TestCase("$FIX [1], [Depth], [5], [MaxWarning]", "I'm starting to get distant - sensor value: 5")]
    [TestCase("$FIX [1], [Depth], [7], [MaxAlarm]", "Too Far!")]
    public void When_PrintIsRequired_Then_CheckTheMessage(string message, string expectedMessage)
    {
        // Arrange
        var analyzerManager = new AnalyzeManager();
        
        // Act
        var userMessage = analyzerManager.GetUserMessage(message);
        
        // Assert
        Assert.That(userMessage, Is.EqualTo(expectedMessage));
    }
}