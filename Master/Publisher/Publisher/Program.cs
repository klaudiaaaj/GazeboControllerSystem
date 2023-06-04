

using Publisher.Services;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddRazorPages();

builder.Services.AddScoped<IRabbitMqSender, RabbitMqSender>();

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();