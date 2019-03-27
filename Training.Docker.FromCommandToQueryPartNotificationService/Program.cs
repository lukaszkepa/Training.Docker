using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
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
                configuration.SetBasePath(hostingContext.HostingEnvironment.ContentRootPath)
                    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                    .AddEnvironmentVariables();
            });

            builder.ConfigureServices((hostingContext, services) => {
                services.AddOptions();
                services.Configure<FileLoggerConfig>(hostingContext.Configuration.GetSection("FileLoggerConfig"));
                services.Configure<RabbitMQConfig>(hostingContext.Configuration.GetSection("RabbitMQ"));
                services.Configure<MongoDBConfig>(hostingContext.Configuration.GetSection("MongoDB"));
                services.Configure<SqlServerDBConfig>(hostingContext.Configuration.GetSection("SqlServer"));
                //services.AddSingleton<ILoggerFactory, FileLoggerFactory>();
                services.AddSingleton<IHostedService, MessagesProcessor>();
            });


            await builder.RunConsoleAsync();
        }
    }
}
