using Contracts.Models;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace RabbitmqSubscriber.Services
{
    public class RabbitMqSubscriberService : IRabbitMqSubscriberService
    {
        private readonly IConfiguration _configuration;
        private IConnection _connection;
        private IModel _channel;
        private ConnectionFactory _connectionFactory;
        private string _queueName;
        private readonly ILogger<RabbitMqSubscriberService> _logger;
        private readonly RosContractor ros;
        private readonly TaskCompletionSource<Joystic> _completionSource = new TaskCompletionSource<Joystic>();
        private bool _messageReceived = false;

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
      
        public async Task<Joystic> ExecuteAsyncSingle()
        {
            var consumer = new EventingBasicConsumer(_channel);

            try
            {
                consumer.Received += (ModuleHandle, ea) =>
                {
                    if (!_messageReceived)
                    {
                        // Odczytywanie i deserializacja wiadomości
                        var body = ea.Body;
                        var notificationMessage = Encoding.UTF8.GetString(body.ToArray());
                        Joystic? joystic = JsonConvert.DeserializeObject<Joystic>(notificationMessage);

                        if (joystic != null)
                        {
                            _messageReceived = true;
                            _completionSource.SetResult(joystic);
                        }
                        
                    }
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }

            if (_channel.IsOpen)
            {
                _channel.Close();
                _connection.Close();
            }

            // Czekanie na wynik zadania i zwrócenie obiektu Joystic
            return await _completionSource.Task; ;
        }
      
        private void RabbitMQ_ConnectionShitdown(object sender, ShutdownEventArgs e)
        {
            Console.WriteLine("--> Connection Shutdown");
        }
    }
}
