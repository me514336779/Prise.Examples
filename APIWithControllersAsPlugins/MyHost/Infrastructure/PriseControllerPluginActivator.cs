using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace MyHost.Infrastructure
{
    public class PriseControllerPluginActivator : IControllerActivator
    {
        public PriseControllerPluginActivator()
        {

        }
        public object Create(ControllerContext context)
        {
            
            throw new NotImplementedException();
        }

        public void Release(ControllerContext context, object controller)
        {
            throw new NotImplementedException();
        }
    }
}
