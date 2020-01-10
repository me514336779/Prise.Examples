using System;
using System.Threading.Tasks;
using Contract;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Prise.Plugin;

namespace OrdersControllerPlugin
{
    [Plugin(PluginType =typeof(IFeaturePlugin))]
    public class OrderControllerFeature : IFeaturePlugin
    {
        //private readonly ApplicationPartManager applicationPartManager;
        internal OrderControllerFeature()
        { }

        public string Name => throw new NotImplementedException();

        public string Description => throw new NotImplementedException();
    }
}
