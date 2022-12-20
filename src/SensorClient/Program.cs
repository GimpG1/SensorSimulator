// See https://aka.ms/new-console-template for more information

using Configuration.Struct.Sensor;
using Grpc.Net.Client;
using GrpcReceiverService;
using SensorClient;
using Receiver = GrpcReceiverService.Receiver;

using var channel = GrpcChannel.ForAddress("http://localhost:5146");
var client = new Receiver.ReceiverClient(channel);

var worker = new Worker();
await worker.Read(new SensorReader());
await worker.ResolveSensors();

var cts = new CancellationTokenSource();
Console.CancelKeyPress += (sender, eventArgs) =>
{
    cts.Cancel();
    eventArgs.Cancel = true;
};

while (!cts.Token.IsCancellationRequested)
{
    foreach (var sensor in worker.Sensors)
    {
        await client.NotifyChangeAsync(new NotifyRequest()
        {
            Message = worker.GetMessageToSend(sensor).Message
        });
    }
}