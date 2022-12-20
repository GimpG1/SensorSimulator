using Configuration.Struct.Receiver;

namespace Manager;

public interface IUserInterface
{
    void Print(ISubscriber subscriber);
    string GetUserMessage(string receiverMessage);
    ConsoleColor AnalyzeMessage(string receiverMessage);
}