using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
        private readonly IActionDescriptorChangeProvider actionDescriptorChangeProvider;

        public FeatureController(
            IPluginLoadOptions<IFeaturePlugin> pluginLoadOptions,
            ApplicationPartManager applicationPartManager,
            IActionDescriptorChangeProvider actionDescriptorChangeProvider
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
            //1.Plugins == singleton
            //2.IFeatureServiceCollection => populated by feature.Enable(which holds an IServiceCollection)(addtransient, scoped,,,,)
            //3.Controller => IFeatureServiceProvider ==> IFeatureServiceCollection.BuildProvider()(this provides ALL plugins services)
            //4.Controller get service from provider

            var assemblyPluginLoadContext = DefaultPluginLoadContext<IFeaturePlugin>.FromAssemblyScanResult(pluginToEnable);
            var pluginAssembly = await pluginLoadOptions.AssemblyLoader.LoadAsync(assemblyPluginLoadContext);
            this.applicationPartManager.ApplicationParts.Add(new AssemblyPart(pluginAssembly));
            //var factory = ApplicationPartFactory.GetApplicationPartFactory(pluginAssembly);
            //foreach (var part in factory.GetApplicationParts(pluginAssembly))
            //{
            //    this.applicationPartManager.ApplicationParts.Add(part);
            //}
            // TODO WHY ROUTE 404 ???
            var controllersFromPlugin = pluginAssembly.GetTypes().Where(t => t.BaseType.Name == typeof(ControllerBase).Name);
            var controllerFeature = new ControllerFeature();
            foreach (var controllerFromPlugin in controllersFromPlugin)
            {
                controllerFeature.Controllers.Add(controllerFromPlugin.GetTypeInfo());
            }
            this.applicationPartManager.PopulateFeature(controllerFeature);
            this.controllerFeatureProvider.PopulateFeature(new[] { new AssemblyPart(pluginAssembly) }, controllerFeature);

            //ActionDescriptorChangeProvider.Instance.HasChanged = true;
            this.actionDescriptorChangeProvider.GetChangeToken();

            return new OkResult();
        }
    }
}
