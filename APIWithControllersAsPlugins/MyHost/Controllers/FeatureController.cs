using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.Extensions.Logging;
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
        private readonly IPluginLoader<IFeaturePlugin> pluginLoader;
        private readonly ApplicationPartManager applicationPartManager;
        private readonly IFeatureServiceProvider featureServiceProvider;

        public FeatureController(
            IPluginLoader<IFeaturePlugin> pluginLoader,
            ApplicationPartManager applicationPartManager,
            IFeatureServiceProvider featureServiceProvider)
        {
            this.pluginLoader = pluginLoader;
            this.applicationPartManager = applicationPartManager;
            this.featureServiceProvider = featureServiceProvider;
        }

        [HttpGet]
        public async Task<IEnumerable<Plugin>> Get()
        {
            var plugins = await this.pluginLoader.LoadAll();

            return plugins.Select(p => new Plugin
            {
                Name = p.Name,
                Description = p.Description
            });
        }

        [HttpPost]
        public async Task<ActionResult> Enable([FromQuery] string name)
        {
            var plugins = await this.pluginLoader.LoadAll();

            var pluginToEnable = plugins.FirstOrDefault(p => p.Name == name);
            if (pluginToEnable == null)
                return new NotFoundResult();
            //1.Plugins == singleton
            //2.IFeatureServiceCollection => populated by feature.Enable(which holds an IServiceCollection)(addtransient, scoped,,,,)
            //3.Controller => IFeatureServiceProvider ==> IFeatureServiceCollection.BuildProvider()(this provides ALL plugins services)
            //4.Controller get service from provider

           await pluginToEnable.EnableFeature(applicationPartManager, featureServiceProvider);

            return plugins.Select(p => new Plugin
            {
                Name = p.Name,
                Description = p.Description
            });
        }
    }
}
