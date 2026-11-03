using Microsoft.AspNetCore.Mvc;

namespace MazeGenerator.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
