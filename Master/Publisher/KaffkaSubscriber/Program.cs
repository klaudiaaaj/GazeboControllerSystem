var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

//konfiguracja kaffki
var hostBuilder = new HostBuilder()
       .ConfigureServices((hostContext, services) =>
       {
                // Konfiguracja us³ug Kafka
                string bootstrapServers = "localhost:9092"; // Adres serwera Kafka
                string topic = "my-topic"; // Nazwa tematu w Kafka
           services.AddHostedService(provider => new KaffkaSubsriberService(bootstrapServers, topic));
       });

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
