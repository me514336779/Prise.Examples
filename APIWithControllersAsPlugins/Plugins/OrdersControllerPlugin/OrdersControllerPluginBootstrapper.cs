using Contract;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Prise.Plugin;

namespace OrdersControllerPlugin
{
    [PluginBootstrapper(PluginType = typeof(OrderControllerFeature))]
    public class OrdersControllerPluginBootstrapper : IPluginBootstrapper
    {
        public IServiceCollection Bootstrap(IServiceCollection services)
        {
            var serviceProvider = services.BuildServiceProvider();
            var featureServices = serviceProvider.GetRequiredService<IFeatureServiceCollection>();
            var config = serviceProvider.GetRequiredService<IConfiguration>();
            var tableStorageConfig = new TableStorageConfig();
            config.Bind("TableStoragePlugin", tableStorageConfig);

            featureServices.AddScoped<TableStorageConfig>(tableStorageConfig);

            return services;
        }
    }
}
