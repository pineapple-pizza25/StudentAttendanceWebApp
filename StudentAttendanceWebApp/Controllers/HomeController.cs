using Microsoft.AspNetCore.Mvc;

namespace StudentAttendanceWebApp.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
