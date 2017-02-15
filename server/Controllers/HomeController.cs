using Microsoft.AspNetCore.Mvc;

namespace MicroSB.Server.Controllers
{

    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

    }
}
