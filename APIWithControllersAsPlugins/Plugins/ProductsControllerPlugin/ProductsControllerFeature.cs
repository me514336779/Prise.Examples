using System;
using Contract;
using Prise.Plugin;

namespace ProductsControllerPlugin
{
    [Plugin(PluginType = typeof(IControllerFeaturePlugin))]
    public class ProductsControllerFeature : IControllerFeaturePlugin
    {
    }
}
