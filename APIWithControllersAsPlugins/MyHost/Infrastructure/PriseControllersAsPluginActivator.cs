using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.DependencyInjection;
using Prise;
using Prise.Infrastructure;
using Prise.Plugin;

namespace MyHost.Infrastructure
{
    public class PriseControllersAsPluginActivator : IControllerActivator
    {
        public object Create(ControllerContext context)
        {
            var pluginLoadOptions = context.HttpContext.RequestServices.GetRequiredService<IPluginLoadOptions<IFeaturePlugin>>();
            var cache = context.HttpContext.RequestServices.GetRequiredService<PrisePluginCache>();
            var controllerType = context.ActionDescriptor.ControllerTypeInfo.AsType();

            foreach (var pluginAssembly in cache.Get())
            {
                if (pluginAssembly.GetTypes().Any(t => t.Name == controllerType.Name))
                {
                    // This will use the parameterless ctor
                    // But it should use the IFeatureServiceProvider from the IFeatureServiceCollection
                    var remoteController = pluginLoadOptions.Activator.CreateRemoteInstance(
                        controllerType,
                        null,
                        null,
                        pluginAssembly);
                    // TODO Unable to cast object of type 'ProductsControllerPlugin.ProductsController' to type 'Microsoft.AspNetCore.Mvc.ControllerBase'.
                    return remoteController;
                }
            }
            var localController = context.HttpContext.RequestServices.GetRequiredService(controllerType);
            // load from default
            return localController;
        }

        public void Release(ControllerContext context, object controller)
        {
            //throw new NotImplementedException();
        }
    }
}
