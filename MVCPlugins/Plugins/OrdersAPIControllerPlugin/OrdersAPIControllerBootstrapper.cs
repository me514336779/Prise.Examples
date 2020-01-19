using Contract;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Prise.Plugin;

namespace OrdersAPIControllerPlugin
{
    [PluginBootstrapper(PluginType = typeof(OrdersAPIController))]
    public class OrdersAPIControllerBootstrapper : IPluginBootstrapper
    {
        public IServiceCollection Bootstrap(IServiceCollection services)
        {
            var config = services.BuildServiceProvider().GetRequiredService<IConfiguration>();
            var ordersConfig = new OrdersConfig();
            config.Bind("Orders", ordersConfig);

            services.AddScoped<OrdersConfig>((serviceProvider) =>
            {
                return ordersConfig;
            });

            return services;
        }
    }
}
