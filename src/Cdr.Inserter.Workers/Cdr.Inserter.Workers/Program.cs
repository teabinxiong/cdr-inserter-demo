using Cdr.Inserter.Workers;
using Cdr.Inserter.Workers.ApplicationServices;
using Cdr.Inserter.Workers.ApplicationServices.WorkerServices;
using Cdr.Inserter.Workers.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Serilog;


var builder = new HostBuilder()
         .ConfigureAppConfiguration((hostingContext, config) =>
         {
             config.AddJsonFile("appsettings.json", optional: true);
             config.AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true);
             config.AddEnvironmentVariables();
             if (args != null)
             {
                 config.AddCommandLine(args);
             }
         })
         .ConfigureServices(s =>
         {
             s.AddSingleton<Cdr.Inserter.Workers.ApplicationServices.BackgroundService>();
             s.AddSingleton<ServicesManager>();
             s.AddSingleton<CdrFileWatcherService>();
             s.AddTransient<CdrFileProcessingService>();
             s.AddSingleton<KafkaClientHandle>();
             s.AddSingleton<KafkaDependentProducer<string, string>>();
         })
         .UseSerilog();


using (IHost host = builder.Build())
{
    var svc = host.Services.GetRequiredService<Cdr.Inserter.Workers.ApplicationServices.BackgroundService>();

    Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(host.Services.GetRequiredService<IConfiguration>())
    .MinimumLevel.Verbose()
    .CreateLogger();
     
    Global.Logger = Log.Logger;
    Global.Logger.Information("Start....");

    host.Start();
    svc.Start();


    host.WaitForShutdown();
    Global.Logger.Information("Received close Signal");
    Console.WriteLine("Received close Signal");
    svc.Stop();

}

Global.Logger.Information("Returning exit code from Program.Main.");
return 123;
