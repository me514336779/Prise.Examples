using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Contract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Prise;
using Prise.Infrastructure;
using Prise.Mvc.Infrastructure;

namespace MyHost.Controllers
{
    public class Feature
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }

    [ApiController]
    [Route("[controller]")]
    public class FeaturesController : ControllerBase
    {
        private readonly IPluginLoadOptions<IControllerFeaturePlugin> pluginLoadOptions;
        private readonly ApplicationPartManager applicationPartManager;
        private readonly IPriseActionDescriptorChangeProvider pluginChangeProvider;
        private readonly IPluginCache<IControllerFeaturePlugin> pluginCache;

        public FeaturesController(
            IPluginLoadOptions<IControllerFeaturePlugin> pluginLoadOptions,
            ApplicationPartManager applicationPartManager,
            IPriseActionDescriptorChangeProvider pluginChangeProvider,
            IPluginCache<IControllerFeaturePlugin> pluginCache
            )
        {
            this.applicationPartManager = applicationPartManager;
            this.pluginLoadOptions = pluginLoadOptions;
            this.pluginChangeProvider = pluginChangeProvider;
            this.pluginCache = pluginCache;
        }

        [HttpGet]
        public async Task<IEnumerable<Feature>> Get()
        {
            var pluginAssemblies = await this.pluginLoadOptions.AssemblyScanner.Scan();

            return pluginAssemblies.Select(p => new Feature
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

            var assemblyPluginLoadContext = DefaultPluginLoadContext<IControllerFeaturePlugin>.FromAssemblyScanResult(pluginToEnable);
            var pluginAssembly = await pluginLoadOptions.AssemblyLoader.LoadAsync(assemblyPluginLoadContext);

            this.applicationPartManager.ApplicationParts.Add(new AssemblyPart(pluginAssembly));
            this.pluginCache.Add(pluginAssembly);
            this.pluginChangeProvider.TriggerPluginChanged();

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
            await pluginLoadOptions.AssemblyLoader.UnloadAsync();
            this.pluginCache.Remove(pluginAssemblyToDisable);
            this.pluginChangeProvider.TriggerPluginChanged();

            return new OkResult();
        }
    }
}
