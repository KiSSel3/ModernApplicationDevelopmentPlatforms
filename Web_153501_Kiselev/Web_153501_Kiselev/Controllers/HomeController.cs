using Microsoft.AspNetCore.Mvc;

namespace Web_153501_Kiselev.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
