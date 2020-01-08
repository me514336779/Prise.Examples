using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contract;
using Microsoft.Extensions.DependencyInjection;

namespace MyHost.Infrastructure
{
    public class FeatureServiceCollection : IFeatureServiceCollection
    {
        private readonly IServiceCollection services;

        public FeatureServiceCollection()
        {
            services = new ServiceCollection();
        }
        public void AddScoped<T>(T implementation)
            where T : class
        {
            services.AddScoped<T>(s => implementation);
        }
    }
}
