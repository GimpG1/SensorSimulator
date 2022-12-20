using Configuration.Struct.Receiver;
using Configuration.Struct.Sensor;
using Devices.Receiver;
using Devices.Sensor;
using Manager;

namespace SensorConsoleAppVariant;

public class Worker
{
    private readonly BuilderManager _builderManager;
    private readonly AnalyzeManager _analyzeManager;

    public Worker(BuilderManager builderManager, IUserInterface analyzeManager)
    {
        _builderManager = builderManager;
        _analyzeManager = (analyzeManager as AnalyzeManager)!;
    }

    public int MessagesCount { get; set; }

    public async Task Build(IReceiverBuilder receiverBuilder, ISensorBuilder sensorBuilder)
    {
        await Task.Run(() =>
        {
            _builderManager.Append(receiverBuilder);
            _builderManager.Append(sensorBuilder);
        });
    }

    public async Task Read(ReceiverReader receiverReader, SensorReader sensorReader)
    {
        try
        {
            await receiverReader.ReadAsync();
            await Task.Delay(100);
            _builderManager.Resolve(receiverReader.Config);
            await sensorReader.ReadAsync();
            await Task.Delay(100);
            _builderManager.Resolve(sensorReader.Config);
        }
        catch (ArgumentException e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task Print(CancellationToken cancellationToken)
    {
        await Task.Run(() =>
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                foreach (var receiver in _builderManager.GetReceivers())
                {
                    _analyzeManager.Print((ISubscriber)receiver);
                    MessagesCount++;
                }
            }
        });
    }
}