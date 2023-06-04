using Confluent.Kafka;
using Contracts.Models;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

public class KaffkaSubsriberService : BackgroundService
{
    private readonly string bootstrapServers;
    private readonly string topic;

    public KaffkaSubsriberService(string bootstrapServers, string topic)
    {
        this.bootstrapServers = bootstrapServers;
        this.topic = topic;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var config = new ConsumerConfig
        {
            BootstrapServers = bootstrapServers,
            GroupId = "my-consumer-group",
            AutoOffsetReset = AutoOffsetReset.Earliest
        };

        using (var consumer = new ConsumerBuilder<Ignore, Joystic>(config).Build())
        {
            consumer.Subscribe(topic);

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var consumeResult = consumer.Consume(stoppingToken);
                    Console.WriteLine($"Odebrano wiadomość z Kafka. Temat: {consumeResult.Topic}, Partycja: {consumeResult.Partition}, Offset: {consumeResult.Offset}, Wiadomość: {consumeResult.Message.Value}");
                }
                catch (OperationCanceledException)
                {
                    // Zakończono oczekiwanie na nową wiadomość
                }
                catch (ConsumeException ex)
                {
                    Console.WriteLine($"Wystąpił błąd podczas konsumowania wiadomości z Kafka: {ex.Error.Reason}");
                }
            }

            consumer.Close();
        }
    }
}
