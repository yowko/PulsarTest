using DotPulsar;
using DotPulsar.Abstractions;
using DotPulsar.Extensions;

namespace PulsarTest;

public class ReaderWithoutBuilderService : BackgroundService
{
    private const string Topic = "persistent://public/default/yowkotest";
    private readonly IPulsarClient _pulsarClient;


    public ReaderWithoutBuilderService(IPulsarClient pulsarClient)
    {
        _pulsarClient = pulsarClient;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var options = new ReaderOptions<string>(MessageId.Earliest, Topic, Schema.String);
            await using var pulsarReader = _pulsarClient.CreateReader(options);
            await foreach (var message in pulsarReader.Messages(stoppingToken))
            {
                Console.WriteLine($"Received: {message.Value()}");
            }

            await Task.Delay(TimeSpan.FromSeconds(3), stoppingToken);
        }
    }
}