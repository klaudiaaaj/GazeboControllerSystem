
using RESTClient.cs.Controllers;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();

builder.Services.AddSwaggerGen();

builder.Services.AddHttpClient<ClientController>();

builder.Host.UseSerilog((ctx, lc) => lc
    .WriteTo.Console()
    .WriteTo.Seq("http://localhost:5341"));

var app = builder.Build();

app.MapGet("/", () => "Hello World!");
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Configure the HTTP request pipeline.
app.MapControllers();

app.MapRazorPages();

app.Run();
