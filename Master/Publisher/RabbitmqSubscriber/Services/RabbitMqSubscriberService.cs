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
        private string _queueName;
        private readonly ILogger<RabbitMqSubscriberService> _logger;
        private readonly RosContractor ros;

        public RabbitMqSubscriberService(IConfiguration configuration, ILogger<RabbitMqSubscriberService> logger)
        {
            _configuration = configuration;
            _logger = logger;
            InitializeRabbitMQ();
            ros = new RosContractor();
        }

        private void InitializeRabbitMQ()
        {
            var ets = _configuration["RabbitMQHost"];
            var jsj = _configuration["RabbitMQPort"];
            _logger.LogInformation(ets);
            _logger.LogInformation(jsj);
            _connectionFactory = new ConnectionFactory() { HostName = _configuration["RabbitMQHost"], Port = int.Parse(_configuration["RabbitMQPort"]) };
            _connection = _connectionFactory.CreateConnection();
            _channel = _connection.CreateModel();

            _queueName = "joystic-queue";
            _channel.QueueBind(queue: _queueName, routingKey: "joystic-queue", exchange: "amq.topic");

            Console.WriteLine("--> Listenting on the Message Bus...");

            _connection.ConnectionShutdown += RabbitMQ_ConnectionShitdown;
        }
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            var consumer = new EventingBasicConsumer(_channel);

            consumer.Received += (ModuleHandle, ea) =>
            {
                var body = ea.Body;
                var notificationMessage = Encoding.UTF8.GetString(body.ToArray());
                //  _eventProcessor.ProcessEvent(notificationMessage);
                _logger.LogInformation("--> Event Received!", notificationMessage.ToString());
            };

            _channel.BasicConsume(queue: _queueName, autoAck: true, consumer: consumer);

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
        private void RabbitMQ_ConnectionShitdown(object sender, ShutdownEventArgs e)
        {
            Console.WriteLine("--> Connection Shutdown");
        }
    }
}
