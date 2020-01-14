using System;
using System.Collections.Generic;
using Contract;
using System.Text;
using Prise.Plugin;

namespace DashboardControllerPlugin
{
    [Plugin(PluginType = typeof(IControllerFeaturePlugin))]
    public class DashboardControllerFeature : IControllerFeaturePlugin
    {
    }
}

