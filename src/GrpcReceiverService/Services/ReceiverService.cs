using Grpc.Core;
using Manager;

namespace GrpcReceiverService.Services;

public class ReceiverService : Receiver.ReceiverBase
{
    private readonly ILogger<ReceiverService> _logger;
    private readonly IUserInterface _analyzeManager;

    public ReceiverService(
        ILogger<ReceiverService> logger,
        IUserInterface analyzeManager)
    {
        _logger = logger;
        _analyzeManager = analyzeManager;
    }

    public override Task<EmptyReply> NotifyChange(NotifyRequest request, ServerCallContext context)
    {
        if (!string.IsNullOrEmpty(request.Message))
        {
            Console.ForegroundColor = _analyzeManager.AnalyzeMessage(request.Message!);
            Console.WriteLine(_analyzeManager.GetUserMessage(request.Message!), Console.ForegroundColor);
        }
        
        return Task.FromResult(new EmptyReply());
    }
}