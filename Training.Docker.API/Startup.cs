using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Polly;
using Rebus.Config;
using Rebus.Injection;
using Rebus.ServiceProvider;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Reflection;
using Training.Docker.API.Services;
using Training.Docker.API.Settings;

namespace Training.Docker.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions()
                .Configure<MongoDbSettings>(Configuration.GetSection("MongoDB"))
                .Configure<RabbitMqSettings>(Configuration.GetSection("RabbitMQ"));

            services.AddRebus((conf, provider) =>
            {
                var settings = provider.GetRequiredService<IOptions<RabbitMqSettings>>();
                var inputQueueName = settings.Value.QueueName ?? Assembly.GetExecutingAssembly().GetName().Name;

                conf.Logging(log => log.Serilog())
                    .Transport(t => t.UseRabbitMq(settings.Value.ConnectionString, inputQueueName));
                return conf;
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddSwaggerGen(s => s.SwaggerDoc("v1", new Info { Title = "Docker Training API", Version = "v1" }));

            services.AddScoped<ICustomerOrdersService, CustomerOrdersService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            var policy = Policy.Handle<ResolutionException>()
                .WaitAndRetry(3, attempt => TimeSpan.FromSeconds(attempt * 5));

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseMvc();

            app.UseSwagger()
               .UseSwaggerUI(s => s.SwaggerEndpoint("/swagger/v1/swagger.json", "Docker Training API"));

            policy.Execute(() => app.UseRebus());
        }
    }
}
