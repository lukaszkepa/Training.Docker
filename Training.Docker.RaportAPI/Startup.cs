using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using Training.Docker.RaportAPI.Data;

namespace Training.Docker.RaportAPI
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
            services.AddDbContext<ApiContext>(cfg =>
                cfg.UseSqlServer(Configuration.GetSection("SqlServer").GetValue<string>("ConnectionString"))
            );
            services.AddSwaggerGen(s => s.SwaggerDoc("v1", new Info { Title = "Docker Training Report API", Version = "v1" }));

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
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
               .UseSwaggerUI(s => s.SwaggerEndpoint("/swagger/v1/swagger.json", "Docker Training Report API"));
        }
    }
}
