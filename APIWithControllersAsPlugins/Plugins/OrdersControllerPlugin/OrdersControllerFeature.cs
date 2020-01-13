using System;
using System.Threading.Tasks;
using Contract;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Prise.Plugin;

namespace OrdersControllerPlugin
{
    [Plugin(PluginType =typeof(IControllerFeaturePlugin))]
    public class OrdersControllerFeature : IControllerFeaturePlugin
    {
    }
}
