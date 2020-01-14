using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;

namespace DashboardControllerPlugin
{
    public class InMemoryTempDataProvider : ITempDataProvider
    {
        private readonly Dictionary<string, object> tempData;
        public InMemoryTempDataProvider()
        {
            this.tempData = new Dictionary<string, object>();
        }

        public IDictionary<string, object> LoadTempData(HttpContext context)
        {
            return this.tempData;
        }

        public void SaveTempData(HttpContext context, IDictionary<string, object> values)
        {
            foreach (var keyValue in values)
                this.tempData[keyValue.Key] = keyValue.Value;
        }
    }

    [Route("dashboard")]
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            //var test = HttpContext.RequestServices.GetRequiredService<IActionResultExecutor<ViewResult>>();
            return View("Index");
        }
    }
}
