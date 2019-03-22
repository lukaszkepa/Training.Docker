using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Rebus.Routing.TypeBased;
using Rebus.ServiceProvider;
using Rebus.Transport.InMem;
using Training.Docker.API.Services;
using Swashbuckle.AspNetCore.Swagger;
using Training.Docker.API.Settings;
using Training.Docker.Models;

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
            var inputQueueName = Assembly.GetExecutingAssembly().GetName().Name;

            services.AddOptions()
                .Configure<MongoDbSettings>(Configuration.GetSection("MongoDB"));

            services.AddRebus(conf =>
            {
                conf.Transport(t => t.UseInMemoryTransport(new InMemNetwork(true), inputQueueName));
                return conf;
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddSwaggerGen(s => s.SwaggerDoc("v1", new Info { Title = "Docker Training API", Version = "v1" }));

            services.AddScoped<ICustomerOrdersService, CustomerOrdersService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseMvc();

            app.UseSwagger()
               .UseSwaggerUI(s => s.SwaggerEndpoint("/swagger/v1/swagger.json", "Docker Training API"));

            app.UseRebus();
        }
    }
}
