using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace RabbitmqSubscriber.Services
{
    public class RabbitMqSubscriberService : BackgroundService
    {
        private readonly IConfiguration _configuration;
        private IConnection _connection;
        private IModel _channel;
        private ConnectionFactory _connectionFactory;

        public RabbitMqSubscriberService(IConfiguration configuration)
        {
            _configuration = configuration;
            InitializeRabbitMQ();
        }

        private void InitializeRabbitMQ()
        {
             _connectionFactory = new RabbitMQ.Client.ConnectionFactory
            {
                Uri = new Uri("amqp://guest:guest@localhost:15672/")
            };

          

        }
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using var connection = _connectionFactory.CreateConnection();
            using var channel = connection.CreateModel();

            channel.QueueDeclare(queue: "test-queue",
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            var consumer = new AsyncEventingBasicConsumer(channel);

            consumer.Received += async (model, ea) =>
            {
                // Arrival time 
                var arrivalTime = DateTime.Now;

                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
            };

            channel.BasicConsume(queue: "joystic-queue",
                                 autoAck: true,
                                 consumer: consumer);
            //Add the service
            return Task.CompletedTask;
        }

        public override void Dispose()
        {
            if (_channel.IsOpen)
            {
                _channel.Close();
                _connection.Close();
            }

            base.Dispose();
        }
    }
}
