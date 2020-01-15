using Microsoft.AspNetCore.Mvc;

namespace DashboardControllerPlugin
{
    [Route("dashboard")]
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View("Index");
        }
    }
}