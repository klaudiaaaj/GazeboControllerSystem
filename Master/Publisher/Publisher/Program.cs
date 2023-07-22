using Publisher.Services;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();


var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog((ctx, lc) => lc
    .WriteTo.Console()
    .WriteTo.Seq("http://localhost:5341"));


builder.Services.AddRazorPages();

builder.Services.AddScoped<IRabbitMqSender, RabbitMqSender>();
builder.Services.AddScoped<IKaffkaSender, KaffkaSender>();
builder.Services.AddScoped<IAzureServiceBusSender, AzureServiceBusSender>();
builder.Services.AddScoped<IDataProducerService, DataProducerService>();
builder.Services.AddSingleton<ISqLiteRepo, SqLiteRepo>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.MapGet("/", () => "Hello World!");
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.MapControllers();
app.Run();

Log.Information("THERE SERRILOG");