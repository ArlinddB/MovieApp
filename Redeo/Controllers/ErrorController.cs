using Microsoft.AspNetCore.Mvc;

namespace Redeo.Controllers
{
    public class ErrorController : Controller
    {
        public IActionResult NotFound()
        {
            return View();
        }
    }
}
