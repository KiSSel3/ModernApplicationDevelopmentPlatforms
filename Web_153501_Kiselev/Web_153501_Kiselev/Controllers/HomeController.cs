using Microsoft.AspNetCore.Mvc;

namespace Web_153501_Kiselev.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            ViewData["Header"] = "Лабораторная работа №2";
            return View();
        }
    }
}
