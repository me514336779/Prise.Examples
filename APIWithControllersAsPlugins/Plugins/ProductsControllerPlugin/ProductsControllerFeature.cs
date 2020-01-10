using System;
using Contract;
using Prise.Plugin;

namespace ProductsControllerPlugin
{
    [Plugin(PluginType = typeof(IFeaturePlugin))]
    public class ProductsControllerFeature : IFeaturePlugin
    {
        internal ProductsControllerFeature()
        { }

        public string Name => throw new NotImplementedException();

        public string Description => throw new NotImplementedException();
    }
}
