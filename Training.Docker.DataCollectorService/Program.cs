using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Rebus.Config;
using Rebus.ServiceProvider;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Training.Docker.Models;

namespace Training.Docker.DataCollectorService
{
    public class Program
    {
        private static readonly string ConsoleLogTemplate = $"[{{Timestamp:yyyy-MM-dd HH:mm:ss}} {{Level:u3}}] {{Message:lj}}{{NewLine}}{{Exception}}";

        static async Task Main(string[] args)
        {
            Log.Logger = BuildLoggerConfiguration().CreateLogger();

            var host = new HostBuilder()
                .ConfigureAppConfiguration(conf =>
                {
                    conf.SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                        .AddEnvironmentVariables(prefix: "ASPNETCORE_");
                })
                .ConfigureServices((context, services) =>
                {
                    services.AddOptions()
                            .Configure<RabbitMqSettings>(context.Configuration.GetSection("RabbitMQ"));

                    services.AddRebus((conf, provider) =>
                    {
                        var settings = provider.GetRequiredService<IOptions<RabbitMqSettings>>();
                        var inputQueueName = settings.Value.QueueName ?? Assembly.GetExecutingAssembly().GetName().Name;

                        conf.Logging(log => log.Serilog())
                            .Transport(t => t.UseRabbitMq(settings.Value.ConnectionString, inputQueueName));
                        return conf;
                    });
                    services.AutoRegisterHandlersFromAssemblyOf<CustomerOrderHandler>();
                })
                .ConfigureLogging(log => log.AddSerilog())
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseConsoleLifetime()
                .Build();

            host.Services.UseRebus(bus => bus.Subscribe<CustomerOrderAdded>().Wait());

            await host.RunAsync();
        }

        private static LoggerConfiguration BuildLoggerConfiguration() =>
            new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .MinimumLevel.Override("System", LogEventLevel.Warning)
                .WriteTo.Console(outputTemplate: ConsoleLogTemplate, theme: AnsiConsoleTheme.Code);
    }
}
