using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Training.Docker.FromCommandToQueryPartNotificationService.Logging;
using Training.Docker.FromCommandToQueryPartNotificationService.MessageProcessing;

namespace Training.Docker.FromCommandToQueryPartNotificationService
{
    class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = new HostBuilder();

            builder.ConfigureAppConfiguration((hostingContext, configuration) => {
                configuration.SetBasePath(hostingContext.HostingEnvironment.ContentRootPath);
                configuration.AddJsonFile("/RabbitMQListenerGenericHost/config/appsettings.json");
            });

            builder.ConfigureServices((hostingContext, services) => {
                services.AddOptions();
                services.Configure<FileLoggerConfig>(hostingContext.Configuration.GetSection("FileLoggerConfig"));
                services.Configure<RabbitMQConfig>(hostingContext.Configuration.GetSection("RabbitMQConfig"));
                services.Configure<MongoDBConfig>(hostingContext.Configuration.GetSection("MongoDBConfig"));
                services.Configure<SqlServerDBConfig>(hostingContext.Configuration.GetSection("SqlServerDBConfig"));
                services.AddSingleton<ILoggerFactory, FileLoggerFactory>();
                services.AddSingleton<IHostedService, MessagesProcessor>();
            });


            await builder.RunConsoleAsync();
        }
    }
}
