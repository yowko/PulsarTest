using System.Text;
using Pulsar.Client.Api;
using Pulsar.Client.Common;

namespace Pulsar.ClientTest;

public class ReaderService : BackgroundService
{
    private const string Topic = "yowkotest";
    private readonly PulsarClient _pulsarClient;


    public ReaderService(PulsarClient pulsarClient)
    {
        _pulsarClient = pulsarClient;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await using var pulsarReader = await _pulsarClient.NewReader(Schema.STRING(Encoding.UTF8))
                .Topic(Topic)
                .StartMessageId(MessageId.Earliest)
                .CreateAsync();

            var message = await pulsarReader.ReadNextAsync(stoppingToken);
            Console.WriteLine($"Received: {message.GetValue()}");
        }
    }
}