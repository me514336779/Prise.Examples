using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.Extensions.DependencyInjection;

namespace Contract
{
    public interface IFeatureServiceCollection
    {
        void AddScoped<T>(T implementation)
            where T : class;
    }

    public interface IFeaturePlugin
    {
        Task EnableFeature(ApplicationPartManager partManager, IFeatureServiceProvider featureServiceProvider);
        Task DisableFeature();

        string Name { get; }
        string Description { get; }
    }

    public interface IFeatureServiceProvider
    {
        IFeatureServiceProvider AddService<T>(ServiceLifetime serviceLifetime);
        T GetService<T>();
    }
}
