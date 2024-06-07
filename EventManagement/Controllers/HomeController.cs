using Microsoft.AspNetCore.Mvc;

namespace EventManagement.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Event");
            }

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        
    }
}
