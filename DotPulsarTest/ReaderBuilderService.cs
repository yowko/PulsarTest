using DotPulsar;
using DotPulsar.Abstractions;
using DotPulsar.Extensions;

namespace PulsarTest;

public class ReaderBuilderService : BackgroundService
{
    private const string Topic = "persistent://public/default/yowkotest";
    private readonly IPulsarClient _pulsarClient;


    public ReaderBuilderService(IPulsarClient pulsarClient)
    {
        _pulsarClient = pulsarClient;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await using var pulsarReader = _pulsarClient.NewReader(Schema.String)
                .StartMessageId(MessageId.Earliest)
                .Topic(Topic)
                .Create();

            await foreach (var message in pulsarReader.Messages(stoppingToken))
            {
                Console.WriteLine($"Received: {message.Value()}");
            }

            await Task.Delay(TimeSpan.FromSeconds(3), stoppingToken);
        }
    }
}