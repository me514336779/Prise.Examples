using System;
using System.Threading.Tasks;
using Contract;
using Microsoft.AspNetCore.Mvc.ApplicationParts;

namespace OrdersControllerPlugin
{
    public class OrderControllerFeature : IFeaturePlugin
    {
        //private readonly ApplicationPartManager applicationPartManager;
        internal OrderControllerFeature()
        {
            //this.applicationPartManager = applicationPartManager;
        }

        public async Task EnableFeature()
        {
            //this.applicationPartManager
        }

        public async Task DisableFeature()
        {
            throw new NotImplementedException();
        }
    }
}
