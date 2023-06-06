using Contracts.Models;
using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;
using RabbitmqSubscriber.Services;
using System.Text;

namespace Publisher.Services
{
    public class RabbitMqSender : IRabbitMqSender
    {
        private readonly IConfiguration _configuration;
        private ConnectionFactory _connectionFactory;
        private readonly ILogger<RabbitMqSender> _logger;

        public RabbitMqSender(IConfiguration configuration, ILogger<RabbitMqSender> logger)
        {
            _configuration = configuration;
            _logger = logger;
            InitializeRabbitMQ();
        }
        private void InitializeRabbitMQ()
        {
            _connectionFactory = new ConnectionFactory() { HostName = _configuration["RabbitMQHost"], Port = int.Parse(_configuration["RabbitMQPort"]) };

            using var connection = _connectionFactory.CreateConnection();
            using var channel = connection.CreateModel();

          //  _channel.ExchangeDeclare(exchange: "trigger", type: ExchangeType.Fanout);
          }

        public async Task Send(IList<Joystic> message)
        {
            var ros = new RosContractor();
            using var connection = _connectionFactory.CreateConnection();
            using var channel = connection.CreateModel();
            try
            {
                foreach (Joystic joystic in message)
                {
                    var id = Guid.NewGuid();
                     ros.GazeboContractor(String.Join(",", joystic.time, joystic.axis_1, joystic.axis_2, joystic.button_1, joystic.button_2));
                    //channel.BasicPublish(exchange: "amq.topic",
                    //                                routingKey: "joystic-queue",
                    //                                basicProperties: null,
                    //                                body: Encoding.UTF8.GetBytes(String.Join(",", joystic.time, joystic.axis_1, joystic.axis_2, joystic.button_1, joystic.button_2, id.ToString())));

                }

            }
            catch (Exception ex)
            {
               _logger.LogError(ex.Message.ToString()); 
            }
         

            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();

        }
    }
}
