using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Prise.Plugin;
using TableStorageConnector;

namespace ProductsReaderPlugin
{
    [PluginBootstrapper(PluginType = typeof(TableStorageProductsReader))]
    public class TableStorageProductsReaderBootstrapper : IPluginBootstrapper
    {
        public IServiceCollection Bootstrap(IServiceCollection services)
        {
            services.AddScoped<TableStorageConfig>((sp) =>
            {
                var pluginServices = sp.GetRequiredService<IPluginServiceProvider>();
                var config = pluginServices.GetHostService<IConfiguration>();
                var tableStorageConfig = new TableStorageConfig();
                config.Bind("TableStoragePlugin", tableStorageConfig);
                return tableStorageConfig;
            });

            return services;
        }
    }
}
