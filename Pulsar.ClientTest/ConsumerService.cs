using System.Text;
using Pulsar.Client.Api;

namespace Pulsar.ClientTest;

public class ConsumerService : BackgroundService
{
    private const string Topic = "yowkotest";
    private const string SubscriptionName = "YowkoSubscription";
    private readonly PulsarClient _pulsarClient;


    public ConsumerService(PulsarClient pulsarClient)
    {
        _pulsarClient = pulsarClient;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await using var pulsarConsumer = await _pulsarClient.NewConsumer(Schema.STRING(Encoding.UTF8))
                .Topic(Topic)
                .SubscriptionName(SubscriptionName)
                .SubscribeAsync();

            var message = await pulsarConsumer.ReceiveAsync(stoppingToken);
            Console.WriteLine($"Received: {message.GetValue()}");
            await pulsarConsumer.AcknowledgeAsync(message.MessageId);
            await Task.Delay(TimeSpan.FromSeconds(3), stoppingToken);
        }
    }
}