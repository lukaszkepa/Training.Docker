using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;

namespace Training.Docker.API
{
    public class Program
    {
        private static readonly string ConsoleLogTemplate = $"[{{Timestamp:yyyy-MM-dd HH:mm:ss}} {{Level:u3}}] {{Message:lj}}{{NewLine}}{{Exception}}";

        public static void Main(string[] args)
        {
            Log.Logger = BuildLoggerConfiguration().CreateLogger();
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseSerilog();

        private static LoggerConfiguration BuildLoggerConfiguration() =>
            new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .MinimumLevel.Override("System", LogEventLevel.Warning)
                .WriteTo.Console(outputTemplate: ConsoleLogTemplate, theme: AnsiConsoleTheme.Code);
    }
}
