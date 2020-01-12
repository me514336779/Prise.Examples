using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace MyHost.Infrastructure
{
    public class PrisePluginCache
    {
        protected ConcurrentBag<Assembly> loadedPlugins;
        public PrisePluginCache()
        {
            this.loadedPlugins = new ConcurrentBag<Assembly>();

        }

        public void Add(Assembly pluginAssembly)
        {
            this.loadedPlugins.Add(pluginAssembly);
        }

        public void Remove(string assemblyName)
        {
            this.loadedPlugins = new ConcurrentBag<Assembly>(this.loadedPlugins.Where(a=>a.GetName().Name != assemblyName));
        }

        public Assembly[] Get() => this.loadedPlugins.ToArray();
    }
}
