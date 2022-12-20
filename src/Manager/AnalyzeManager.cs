using Configuration.Ranges;
using Configuration.Struct.Receiver;
using Devices.Receiver;

namespace Manager;

public class AnalyzeManager : IUserInterface
{
    public void Print(ISubscriber subscriber)
    {
        if (subscriber is Receiver receiver)
        {
            if (receiver.Message == String.Empty)
            {
                return;
            }
            var color = AnalyzeMessage(receiver.Message!);
            Console.WriteLine(GetUserMessage(receiver.Message!), color);
        }
    }

    public string GetUserMessage(string receiverMessage)
    {
        var message = string.Empty;
        var value = GetValue(receiverMessage);
        var statusMessage = GetStateOfMessage(receiverMessage);
        
        message = statusMessage switch
        {
            State.MinWarning => $"/_\\ I'm getting closer so fast - sensor value:{value}",
            State.MaxWarning => $"/_\\ I'm starting to get distant - sensor value:{value}",
            State.MinAlarm => "(!) Too Close!",
            State.MaxAlarm => "(!) Too Far!",
            State.Normal => "I'm ok",
            _ => string.Empty
        };
        return message;
    }

    public ConsoleColor AnalyzeMessage(string receiverMessage)
    {
        var statusMessage = GetStateOfMessage(receiverMessage);

        var color = statusMessage switch
        {
            State.MinWarning => ConsoleColor.Yellow,
            State.MaxWarning => ConsoleColor.Yellow,
            State.MinAlarm => ConsoleColor.Red,
            State.MaxAlarm => ConsoleColor.Red,
            State.Normal => ConsoleColor.Green,
            _ => ConsoleColor.Black
        };

        return color;
    }

    private string GetValue(string receiverMessage)
    {
        return receiverMessage.Split(",").TakeLast(2).First().Replace("[", "").Replace("]", "");
    }

    private State GetStateOfMessage(string receiverMessage)
    {
        var statusMessage = receiverMessage.Split(",").Last().Replace("[", "").Replace("]", "");
        return Enum.Parse<State>(statusMessage);
    }
}