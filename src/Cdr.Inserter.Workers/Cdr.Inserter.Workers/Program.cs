using Cdr.Inserter.Workers;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Serilog;

var builder = new ConfigurationBuilder();
builder.SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true)
    .AddEnvironmentVariables();

IHost host = Host.CreateDefaultBuilder()
    .ConfigureServices((context, services) =>
    {
    })
    .UseSerilog()
    .Build();

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Build())
    .MinimumLevel.Verbose()
    .CreateLogger();

Global.Logger = Log.Logger;

var configuration = host.Services.GetRequiredService<IConfiguration>();

var svc = new Cdr.Inserter.Workers.ApplicationServices.BackgroundService();
svc.Start();


bool quit = false;

while (!quit)
{
    Thread.Sleep(1000);
    if (Console.KeyAvailable)
    {
        ConsoleKeyInfo key = Console.ReadKey();
        if (key.KeyChar == 'q' || key.KeyChar == 'Q')
        {
            Global.Logger.Information("Quit Program");

            svc.Stop();
        }
    }
}


host.Run();