using System;
using System.Threading.Tasks;
using Contract;
using ExternalServices;
using Language.Domain;
using Microsoft.Extensions.Configuration;
using Prise.Plugin;

namespace LanguageBased.Plugin
{
    [Plugin(PluginType = typeof(IHelloPlugin))]
    public class LanguageBasedPlugin : IHelloPlugin
    {
        private readonly IConfiguration configuration;
        private readonly IExternalService externalService;
        private readonly IDictionaryService dictionaryService;
        protected LanguageBasedPlugin(IConfiguration configuration, IExternalService externalService, IDictionaryService dictionaryService)
        {
            this.configuration = configuration;
            this.externalService = externalService;
            this.dictionaryService = dictionaryService;
        }

        [PluginFactory]
        public static LanguageBasedPlugin ThisIsTheFactoryMethod(IPluginServiceProvider services)
        {
            var configFromHost = services.GetHostService<IConfiguration>();

            var hostService = services.GetSharedHostService(typeof(IExternalService));
            var hostServiceBridge = new ExternalServiceBridge(hostService);

            var dictionaryService = services.GetPluginService<IDictionaryService>();

            return new LanguageBasedPlugin(
                configFromHost, // This service is provided by the MyHost application and is registered as a Host Type
                hostServiceBridge, // This service is provided by the MyHost application and is registered as a Remote Type
                dictionaryService // This service is provided by the plugin using the PluginBootstrapper
            );
        }

        public string SayHello(string input)
        {
            var language = this.externalService.GetExternalObject().Language;
            var dictionary = dictionaryService.GetLanguageDictionary().Result;

            var languageFromConfig = this.configuration["LanuageOverride"];
            if (!String.IsNullOrEmpty(languageFromConfig))
                language = languageFromConfig;

            if (dictionary.ContainsKey(language))
                return $"{dictionary[language]} {input}";

            return $"We could not find a suitable word for language {language}";
        }

        public async Task<string> SayHelloAsync(string input)
        {
            var language = (await this.externalService.GetExternalObjectAsync()).Language;
            var dictionary = await dictionaryService.GetLanguageDictionary();

            var languageFromConfig = this.configuration["LanuageOverride"];
            if (!String.IsNullOrEmpty(languageFromConfig))
                language = languageFromConfig;

            if (dictionary.ContainsKey(language))
                return $"{dictionary[language]} {input}";

            return $"We could not find a suitable word for language {language}";
        }
    }
}
