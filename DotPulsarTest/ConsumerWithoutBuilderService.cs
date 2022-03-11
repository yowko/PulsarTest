using DotPulsar;
using DotPulsar.Abstractions;
using DotPulsar.Extensions;

namespace PulsarTest;

public class ConsumerWithoutBuilderService:BackgroundService
{
    private const string Topic = "persistent://public/default/yowkotest";
     private const string SubscriptionName = "YowkoSubscription";
        private readonly IPulsarClient _pulsarClient;


     public ConsumerWithoutBuilderService(IPulsarClient pulsarClient)
     {
         _pulsarClient = pulsarClient;   
     }

     protected override async Task ExecuteAsync(CancellationToken stoppingToken)
     {
         while (!stoppingToken.IsCancellationRequested)
         {
             var options = new ConsumerOptions<string>(SubscriptionName,Topic,Schema.String);
             await using var pulsarConsumer = _pulsarClient.CreateConsumer(options);
             
             await foreach (var message in pulsarConsumer.Messages(stoppingToken))
             {
                 Console.WriteLine($"Received: {message.Value()}");
                 await pulsarConsumer.Acknowledge(message, stoppingToken);
                 //await pulsarConsumer.AcknowledgeCumulative(message, stoppingToken);
             }
             
             await Task.Delay(TimeSpan.FromSeconds(3), stoppingToken);
         }
     }
}