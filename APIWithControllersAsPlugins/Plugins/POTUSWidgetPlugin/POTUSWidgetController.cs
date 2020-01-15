using Microsoft.AspNetCore.Mvc;

namespace POTUSWidgetPlugin
{
    [Route("potus")]
    // The name of the Views folder must be POTUSWidget
    public class POTUSWidgetController : Controller
    {
        public IActionResult Index()
        {
            return View("Index");
        }
    }
}