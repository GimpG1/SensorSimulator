using Grpc.Core;
using Manager;

namespace GrpcReceiverService.Services;

public class ReceiverService : Receiver.ReceiverBase
{
    private readonly ILogger<ReceiverService> _logger;
    private readonly IConfiguration _configuration;
    private readonly IUserInterface _analyzeManager;
    private int _listenedSensorId = 0;

    public ReceiverService(
        ILogger<ReceiverService> logger,
        IConfiguration configuration,
        IUserInterface analyzeManager)
    {
        _logger = logger;
        _configuration = configuration;
        _analyzeManager = analyzeManager;

        _listenedSensorId = _configuration.GetValue<int>("ListenedSensorId");
    }

    public override Task<EmptyReply> NotifyChange(NotifyRequest request, ServerCallContext context)
    {
        if (!string.IsNullOrEmpty(request.Message))
        {
            if (_listenedSensorId == GetId(request.Message))
            {
                Console.ForegroundColor = _analyzeManager.AnalyzeMessage(request.Message!);
                Console.WriteLine(_analyzeManager.GetUserMessage(request.Message!), Console.ForegroundColor);
            }
        }
        
        return Task.FromResult(new EmptyReply());
    }

    private int GetId(string message)
    {
        int.TryParse(message.Split(",").Take(2).Last().Replace("[", "").Replace("]", ""), out var result);
        return result; 
    }
}