using ClinicaVeterinaria.Models;
using ClinicaVeterinaria.Service.Intertface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace ClinicaVeterinaria.Controllers
{
    public class UtenteController : Controller
    {
        private readonly IUtenteService _utenteService;
        private readonly ILogger<UtenteController> _logger;

        public UtenteController(IUtenteService utenteService, ILogger<UtenteController> logger)
        {
            _utenteService = utenteService;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Register()
        {
            bool isAdmin = User.Identity.IsAuthenticated && User.IsInRole("Admin");
            ViewBag.IsAdmin = isAdmin;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(Utente model)
        {
            _logger.LogInformation("Register attempt for username: {Username}", model.Username);

            if (ModelState.IsValid)
            {
                try
                {
                    // Imposta la password e i valori di hash/salt
                    if (!string.IsNullOrEmpty(model.Password))
                    {
                        model.SetPassword(model.Password);
                    }

                    // Salva l'utente nel database
                    await _utenteService.CreateUtenteAsync(model);
                    _logger.LogInformation("User registered successfully: {Username}", model.Username);
                    return RedirectToAction("Login");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred while registering user: {Username}", model.Username);
                    ModelState.AddModelError(string.Empty, "Errore durante la registrazione.");
                }
            }
            else
            {
                foreach (var error in ModelState)
                {
                    foreach (var errorMessage in error.Value.Errors)
                    {
                        _logger.LogWarning("ModelState error for field {Field}: {Error}", error.Key, errorMessage.ErrorMessage);
                    }
                }
                _logger.LogWarning("Model state is invalid for username: {Username}", model.Username);
            }

            return View(model);
        }
    }
}
