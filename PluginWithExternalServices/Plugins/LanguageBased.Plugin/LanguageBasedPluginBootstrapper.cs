using Microsoft.Extensions.DependencyInjection;
using Prise.Plugin;

namespace LanguageBased.Plugin
{
    [PluginBootstrapper(PluginType = typeof(LanguageBasedPlugin))]
    public class LanguageBasedPluginBootstrapper : IPluginBootstrapper
    {
        public IServiceCollection Bootstrap(IServiceCollection services)
        {
            // Register all services that are required to load the LanguageBasedPlugin
            services.AddScoped<IDictionaryService, DictionaryService>();
            return services;
        }
    }
}
