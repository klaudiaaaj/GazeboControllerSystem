using Confluent.Kafka;
using Contracts.Models;

namespace Publisher.Services
{
    public class KaffkaSender : IKaffkaSender
    {
        private readonly string bootstrapServers;
        private readonly string topic;

        public KaffkaSender(string bootstrapServers, string topic)
        {
            this.bootstrapServers = bootstrapServers;
            this.topic = topic;
        }
        public async Task Send(IList<Joystic> message)
        {
            var config = new ProducerConfig { BootstrapServers = bootstrapServers };
            using (var producer = new ProducerBuilder<Null, Joystic>(config).Build())
            {
                try
                {
                    foreach (Joystic joystic in message)
                    {
                        var id = Guid.NewGuid();
                        var deliveryReport = await producer.ProduceAsync(topic, new Message<Null, Joystic> { Value = joystic });
                        Console.WriteLine($"Wiadomość wysłana do Kafka. Temat: {deliveryReport.Topic}, Partycja: {deliveryReport.Partition}, Offset: {deliveryReport.Offset}");
                    }
                }
                catch (ProduceException<Null, string> ex)
                {
                    Console.WriteLine($"Wystąpił błąd podczas wysyłania wiadomości do Kafka: {ex.Error.Reason}");
                }
            }
        }
    }
}
