using Microsoft.AspNetCore.Mvc;
using Software_Engineering_Project.Models;
using System.Diagnostics;

namespace Software_Engineering_Project.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        // Login page view method
        public IActionResult Login()
        {
            return View();
        }

        // TeacherHome page view method
        public IActionResult TeacherHome()
        {
            return View();
        }
    }
}