using Microsoft.Extensions.Hosting;
using Rebus.ServiceProvider;
using Rebus.Transport.InMem;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace Training.Docker.DataCollectorService
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var inputQueueName = Assembly.GetExecutingAssembly().GetName().Name;

            var host = new HostBuilder()
                .ConfigureServices(services =>
                {
                    services.AddRebus(conf =>
                    {
                        conf.Transport(t => t.UseInMemoryTransport(new InMemNetwork(true), inputQueueName));
                        return conf;
                    });
                })
                .UseConsoleLifetime()
                .Build();

            host.Services.UseRebus();

            await host.RunAsync();
        }
    }
}
