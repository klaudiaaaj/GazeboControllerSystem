using Contracts;
using Contracts.Services;
using MetricMaster;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;


var factory = new ConnectionFactory { Uri = new Uri("amqp://guest:guest@localhost:5672") };

using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();

channel.QueueDeclare(queue: "test-queue",
                     durable: false,
                     exclusive: false,
                     autoDelete: false,
                     arguments: null);

var consumer = new AsyncEventingBasicConsumer(channel);

//Add the service

var someService = app.Services.GetService<MetricsRepository>();

consumer.Received += async (model, ea) =>
{
    // Arrival time 
    var arrivalTime = DateTime.Now;

    var body = ea.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);

    //fire and forget
     ProcessMessageAsync(message, arrivalTime);
};
channel.BasicConsume(queue: "joystic-queue",
                     autoAck: true,
                     consumer: consumer);

Console.WriteLine(" Press [enter] to exit.");
Console.ReadLine();

static async Task ProcessMessageAsync(string message, DateTime arrivalTime)
{
    var messageWithArrivalTime = new MessageWithArrivalTime(message, arrivalTime);
    
    // Wywołanie przetwarzania danych
    PythonCaller pyt = new PythonCaller();
    await pyt.GazeboContractor(messageWithArrivalTime.Message);

    // Tutaj możesz wykorzystać pole arrivalTime, aby zapisywać metryki lub inne operacje na danych
    Console.WriteLine($"Przetworzono wiadomość o przybyciu: {messageWithArrivalTime.ArrivalDate}");
}