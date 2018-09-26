using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Linq;
using System.Threading.Tasks;
using ToolBelt.MobileAppService.Services;

namespace ToolBelt.MobileAppService
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            services.AddDbContext<ToolBeltContext>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "My API", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            // TODO: REMOVE:
            var dbContext = app.ApplicationServices.GetService<ToolBeltContext>();
            AddTestData(dbContext);

            app.UseMvc();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            app.Run(async (context) => await Task.Run(() => context.Response.Redirect("/swagger")));
        }

        private void AddTestData(ToolBeltContext context)
        {
            context.Trades.RemoveRange(context.Trades);

            context.Trades.AddRange(
                new[]
                {
                    "Roofing",
                    "Siding",
                    "Flooring",
                    "Framing",
                    "Dry-wall",
                    "Tiling",
                    "Masonry (stone work)",
                    "Chimney",
                    "Concrete",
                    "Asphalt",
                    "Remodeling (bathrooms)",
                    "Remodeling (kitchens)",
                    "Carpentry (finished)",
                    "Carpentry (rough)",
                    "Painting (interior)",
                    "Painting (exterior)",
                    "Plumbing",
                    "Electrical",
                    "Landscaping",
                    "Appliance Installation",
                    "Window Replacement",
                    "Welding",
                    "Decks",
                    "Fencing",
                    "Tile",
                    "Kitchen Remodeling",
                    "Bathroom Remodeling",
                    "Concrete"
                }
                .Select((specialty, index) => new Data.Trade { Name = specialty }));

            context.SaveChanges();
        }
    }
}
