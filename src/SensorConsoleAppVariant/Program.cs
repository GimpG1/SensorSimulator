// See https://aka.ms/new-console-template for more information

using Configuration.Struct.Receiver;
using Configuration.Struct.Sensor;
using Devices.Receiver;
using Devices.Sensor;
using Manager;
using SensorConsoleAppVariant;

var builderManager = new Worker(new BuilderManager(), new AnalyzeManager());

await builderManager.Build(new ReceiverBuilder(), new SensorBuilder());
await builderManager.Read(new ReceiverReader(), new SensorReader());

var cts = new CancellationTokenSource();
Console.CancelKeyPress += (sender, eventArgs) =>
{
    cts.Cancel();
    eventArgs.Cancel = true;
};
await builderManager.Print(cts.Token);

Console.ForegroundColor = ConsoleColor.Cyan;
Console.WriteLine($"Counted messages {builderManager.MessagesCount}", Console.ForegroundColor);
Console.ReadKey();
