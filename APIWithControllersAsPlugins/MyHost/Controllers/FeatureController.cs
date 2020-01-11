using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Threading.Tasks;
using Contract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Logging;
using MyHost.Infrastructure;
using Prise;
using Prise.Infrastructure;

namespace MyHost.Controllers
{
    public class Plugin
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }

    [ApiController]
    [Route("[controller]")]
    public class FeatureController : ControllerBase
    {
        private readonly IPluginLoadOptions<IFeaturePlugin> pluginLoadOptions;
        private readonly ApplicationPartManager applicationPartManager;
        //private readonly ActionDescriptorChangeProvider changeProvider;
        private readonly ControllerFeatureProvider controllerFeatureProvider;
        //private readonly IFeatureServiceProvider featureServiceProvider;
        private readonly ActionDescriptorChangeProvider actionDescriptorChangeProvider;

        public FeatureController(
            IPluginLoadOptions<IFeaturePlugin> pluginLoadOptions,
            ApplicationPartManager applicationPartManager,
            ActionDescriptorChangeProvider actionDescriptorChangeProvider
            //ActionDescriptorChangeProvider changeProvider
            //IPluginLoader<IFeaturePlugin> pluginLoader,
            //IFeatureServiceProvider featureServiceProvider
            )
        {
            this.applicationPartManager = applicationPartManager;
            this.pluginLoadOptions = pluginLoadOptions;
            this.actionDescriptorChangeProvider = actionDescriptorChangeProvider;
            //this.changeProvider = changeProvider;
            controllerFeatureProvider = (ControllerFeatureProvider)applicationPartManager.FeatureProviders
                .Where(p => p is ControllerFeatureProvider).FirstOrDefault();
        }

        [HttpGet]
        public async Task<IEnumerable<Plugin>> Get()
        {
            var pluginAssemblies = await this.pluginLoadOptions.AssemblyScanner.Scan();

            return pluginAssemblies.Select(p => new Plugin
            {
                Name = p.PluginType.Name
            });
        }

        [HttpPost]
        public async Task<ActionResult> Enable([FromQuery] string name)
        {
            var pluginAssemblies = await this.pluginLoadOptions.AssemblyScanner.Scan();

            var pluginToEnable = pluginAssemblies.FirstOrDefault(p => p.PluginType.Name == name);
            if (pluginToEnable == null)
                return new NotFoundResult();

            var assemblyPluginLoadContext = DefaultPluginLoadContext<IFeaturePlugin>.FromAssemblyScanResult(pluginToEnable);
            // This works, load the assembly into the default load context
            var pluginAssembly = AssemblyLoadContext.Default.LoadFromAssemblyPath(Path.Combine(assemblyPluginLoadContext.PluginAssemblyPath, assemblyPluginLoadContext.PluginAssemblyName));

            // This does not work: 404
            //var pluginAssembly = await pluginLoadOptions.AssemblyLoader.LoadAsync(assemblyPluginLoadContext);

            this.applicationPartManager.ApplicationParts.Add(new AssemblyPart(pluginAssembly));

            this.actionDescriptorChangeProvider.HasChanged = true;
            this.actionDescriptorChangeProvider.TokenSource.Cancel();


            return new OkResult();
        }
    }
}
