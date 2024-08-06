using ClinicaVeterinaria.Models;
using ClinicaVeterinaria.Service;
using ClinicaVeterinaria.Service.Intertface;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ClinicaVeterinaria.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IAnimaleService _animaleService;


        public HomeController(ILogger<HomeController> logger, IAnimaleService animaleService)
        {
            _logger = logger;
            _animaleService = animaleService;
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
    }
}
