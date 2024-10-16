using Microsoft.AspNetCore.Mvc;

namespace RealStats.Controllers
{
    public class TestsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
