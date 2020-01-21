using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Contract;
using ExternalServices;
using Prise.Plugin;

namespace LanguageBased.Plugin
{
    [Plugin(PluginType = typeof(IHelloPlugin))]
    public class LanguageBasedPlugin : IHelloPlugin
    {
        private readonly IExternalService externalService;
        private readonly IDictionaryService dictionaryService;
        protected LanguageBasedPlugin(IExternalService externalService, IDictionaryService dictionaryService)
        {
            this.externalService = externalService;
            this.dictionaryService = dictionaryService;
        }

        [PluginFactory]
        public static LanguageBasedPlugin ThisIsTheFactoryMethod(IPluginServiceProvider services)
        {
            var hostService = services.GetSharedHostService(typeof(IExternalService));
            var hostServiceBridge = new ExternalServiceBridge(hostService);

            var dictionaryService = services.GetPluginService<IDictionaryService>();

            return new LanguageBasedPlugin(
                hostServiceBridge, // This service is provided by the MyHost application
                dictionaryService // This service is provided by the plugin using the PluginBootstrapper
            );
        }

        public string SayHello(string input)
        {
            if (this.externalService == null)
                throw new Exception("externalService is null");

            var language = this.externalService.GetExternalObject().Language;
            var dictionary = dictionaryService.GetDictionary().Result;

            if (dictionary.ContainsKey(language))
                return $"{dictionary[language]} {input}";

            return $"We could not find a suitable word for language {language}";
        }

        public async Task<string> SayHelloAsync(string input)
        {
            if (this.externalService == null)
                throw new Exception("externalService is null");

            var language = (await this.externalService.GetExternalObjectAsync()).Language;
            var dictionary = await dictionaryService.GetDictionary();

            if (dictionary.ContainsKey(language))
                return $"{dictionary[language]} {input}";

            return $"We could not find a suitable word for language {language}";
        }
    }
}
