using ClinicaVeterinaria.Interface;
using ClinicaVeterinaria.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ClinicaVeterinaria.Controllers
{
    [Route("User")]
    public class UtenteController : Controller
    {
        private readonly IUtenteService _utenteService;
        private readonly ILogger<UtenteController> _logger;

        public UtenteController(IUtenteService utenteService, ILogger<UtenteController> logger)
        {
            _utenteService = utenteService;
            _logger = logger;
        }

        [HttpGet("Login")]
        public IActionResult Login()
        {
            return View("~/Views/User/Login.cshtml");
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            _logger.LogInformation("Tentativo di login per l'utente: {Username}", model.Username);

            if (ModelState.IsValid)
            {
                var user = await _utenteService.LoginAsync(model.Username, model.Password);
                if (user != null)
                {
                    // Logica per la gestione delle claims e l'autenticazione
                    var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Ruolo)
            };

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var authProperties = new AuthenticationProperties { };

                    await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity),
                        authProperties);

                    _logger.LogInformation("Login riuscito per l'utente: {Username}", model.Username);
                    return RedirectToAction("Index", "Home");
                }

                _logger.LogWarning("Tentativo di login fallito per l'utente: {Username}", model.Username);
                ModelState.AddModelError(string.Empty, "Username o password errati.");
            }

            return View(model);
        }
        [HttpPost("Logout")]
        public async Task<IActionResult> Logout()
        {
            // Effettua il logout dell'utente
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            // Reindirizza alla homepage
            return RedirectToAction("Index", "Home");
        }
    }
}
