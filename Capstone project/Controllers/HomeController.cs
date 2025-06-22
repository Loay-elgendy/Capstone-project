using Microsoft.AspNetCore.Mvc;

namespace Capstone_project.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Home()
        {
            return View();
        }
    }
}
