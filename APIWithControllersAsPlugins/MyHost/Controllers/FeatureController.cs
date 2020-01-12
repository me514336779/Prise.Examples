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
        private readonly ActionDescriptorChangeProvider actionDescriptorChangeProvider;
        private readonly PrisePluginCache cache;

        public FeatureController(
            IPluginLoadOptions<IFeaturePlugin> pluginLoadOptions,
            ApplicationPartManager applicationPartManager,
            ActionDescriptorChangeProvider actionDescriptorChangeProvider,
            PrisePluginCache cache
            )
        {
            this.applicationPartManager = applicationPartManager;
            this.pluginLoadOptions = pluginLoadOptions;
            this.actionDescriptorChangeProvider = actionDescriptorChangeProvider;
            this.cache = cache;
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
            var pluginAssembly = await pluginLoadOptions.AssemblyLoader.LoadAsync(assemblyPluginLoadContext);

            this.applicationPartManager.ApplicationParts.Add(new AssemblyPart(pluginAssembly));
            this.cache.Add(pluginAssembly);

            this.actionDescriptorChangeProvider.HasChanged = true;
            this.actionDescriptorChangeProvider.TokenSource.Cancel();


            return new OkResult();
        }

        [HttpDelete]
        public async Task<ActionResult> Disable([FromQuery] string name)
        {
            var pluginAssemblies = await this.pluginLoadOptions.AssemblyScanner.Scan();

            var pluginToDisable = pluginAssemblies.FirstOrDefault(p => p.PluginType.Name == name);
            if (pluginToDisable == null)
                return new NotFoundResult();
            var pluginAssemblyToDisable = Path.GetFileNameWithoutExtension(pluginToDisable.AssemblyName);
            var partToRemove = this.applicationPartManager.ApplicationParts.FirstOrDefault(a => a.Name == pluginAssemblyToDisable);
            this.applicationPartManager.ApplicationParts.Remove(partToRemove);
            this.cache.Remove(pluginAssemblyToDisable);

            this.actionDescriptorChangeProvider.HasChanged = true;
            this.actionDescriptorChangeProvider.TokenSource.Cancel();

            return new OkResult();
        }
    }
}
