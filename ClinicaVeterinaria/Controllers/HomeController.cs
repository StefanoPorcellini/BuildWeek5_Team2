using ClinicaVeterinaria.Models;
using ClinicaVeterinaria.Service;
using ClinicaVeterinaria.Service.Intertface;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

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
            // Verifica se l'utente è autenticato
            if (User.Identity.IsAuthenticated)
            {
                // Ottiene il ruolo dell'utente
                var role = User.FindFirst(ClaimTypes.Role)?.Value;

                // Reindirizza l'utente alla dashboard appropriata in base al ruolo
                return role switch
                {
                    "Admin" => RedirectToAction("AdminDashboard", "User"),
                    "Farmacista" => RedirectToAction("FarmacistaDashboard", "User"),
                    "Veterinario" => RedirectToAction("VeterinarioDashboard", "User"),
                    _ => RedirectToAction("Index"), // Reindirizza alla home page se il ruolo non è riconosciuto
                };
            }

            // Se l'utente non è autenticato, mostra la home page
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
