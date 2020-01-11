using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Contract;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MyHost.Controllers;
using MyHost.Infrastructure;
using Prise;
using Prise.AssemblyScanning.Discovery;

namespace MyHost
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
            services.AddControllers();

            services.AddTransient<FeatureController>();
            var provider = new ActionDescriptorChangeProvider();
            services.AddSingleton<IActionDescriptorChangeProvider>(provider);

            services.AddSingleton<ActionDescriptorChangeProvider>(provider);
            services.Replace(ServiceDescriptor.Transient<IControllerActivator, PriseControllersAsPluginActivator>());

            services.AddPrise<IFeaturePlugin>(config =>
                config
                    .WithDefaultOptions(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Plugins"))
                    .ScanForAssemblies(composer =>
                        composer.UseDiscovery())
                    .ConfigureSharedServices(sharedServices =>
                    {
                        sharedServices.AddSingleton(Configuration);
                    }));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
