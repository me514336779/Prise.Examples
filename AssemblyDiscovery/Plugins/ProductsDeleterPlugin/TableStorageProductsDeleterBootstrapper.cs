using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Prise.Plugin;
using TableStorageConnector;

namespace ProductsDeleterPlugin
{
    [PluginBootstrapper(PluginType = typeof(TableStorageProductsDeleter))]
    public class TableStorageProductsDeleterBootstrapper : IPluginBootstrapper
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
