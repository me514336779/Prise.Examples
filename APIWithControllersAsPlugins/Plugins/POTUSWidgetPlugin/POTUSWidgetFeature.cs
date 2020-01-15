using Contract;
using Prise.Plugin;

namespace POTUSWidgetPlugin
{
    [Plugin(PluginType = typeof(IMVCFeature))]
    [MVCFeatureDescription(Description = "This feature will add the '/potus' widget to the current MVC Host.")]
    public class POTUSTwitterPluginFeature : IMVCFeature
    {
        // Nothing to do here, just some feature discovery happening...
    }
}

