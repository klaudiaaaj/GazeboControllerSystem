

using Publisher.Services;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddRazorPages();

builder.Services.AddScoped<IRabbitMqSender, RabbitMqSender>();
builder.Services.AddScoped<IKaffkaSender, KaffkaSender>();

//konfiguracja kaffki
var hostBuilder = new HostBuilder()
          .ConfigureServices((hostContext, services) =>
          {
                // Konfiguracja us³ug Kafka
                string bootstrapServers = "localhost:9092"; // Adres serwera Kafka
                string topic = "my-topic"; // Nazwa tematu w Kafka

                services.AddSingleton(new KaffkaSender(bootstrapServers, topic));
          });

await hostBuilder.RunConsoleAsync();
var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();