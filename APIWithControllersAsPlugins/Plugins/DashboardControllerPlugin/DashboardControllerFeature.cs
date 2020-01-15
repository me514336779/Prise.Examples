using Contract;
using Prise.Plugin;

namespace DashboardControllerPlugin
{
    [Plugin(PluginType = typeof(IControllerFeaturePlugin))]
    public class DashboardControllerFeature : IControllerFeaturePlugin
    {
        // Nothing to do here, just some feature discovery happening...
    }
}

