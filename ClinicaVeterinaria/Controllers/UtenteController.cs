using ClinicaVeterinaria.Models;
using ClinicaVeterinaria.Service;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ClinicaVeterinaria.Controllers
{
    [Route("User")]
    public class UtenteController : Controller
    {
        private readonly IUtenteService _utenteService;

        public UtenteController(IUtenteService utenteService)
        {
            _utenteService = utenteService;
        }

        [HttpGet("Login")]
        public IActionResult Login()
        {
            return View("~/Views/User/Login.cshtml");
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            // Verifica che il ModelState sia valido
            if (ModelState.IsValid)
            {
                var user = await _utenteService.LoginAsync(model.Username, model.Password);
                if (user != null)
                {
                    // Se l'utente è trovato, si creano le claims per l'autenticazione
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, user.Username),
                        new Claim(ClaimTypes.Role, user.Ruolo)
                    };

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var authProperties = new AuthenticationProperties { };

                    // Si effettua l'accesso dell'utente
                    await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity),
                        authProperties);

                    // Reindirizza l'utente alla homepage
                    return RedirectToAction("Index", "Home");
                }

                // Se la combinazione username/password non è valida, aggiungi un errore al ModelState
                ModelState.AddModelError(string.Empty, "Username o password errati.");
            }

            // Se il ModelState non è valido, ritorna alla vista di login con gli errori
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

        [HttpGet("CreateAdmin")]
        public IActionResult CreateAdmin()
        {
            return View("~/Views/User/CreateAdmin.cshtml");
        }

        [HttpPost("CreateAdmin")]
        public async Task<IActionResult> CreateAdmin(CreateAdminViewModel model)
        {
            // Verifica che il ModelState sia valido
            if (ModelState.IsValid)
            {
                try
                {
                    // Creazione di un nuovo oggetto Utente per l'admin
                    var newAdmin = new Utente
                    {
                        Username = model.Username,
                        Ruolo = model.Ruolo
                    };

                    // Imposta la password dell'admin
                    newAdmin.SetPassword(model.Password);

                    // Salva il nuovo admin nel database
                    await _utenteService.CreateUtenteAsync(newAdmin);

                    // Reindirizza alla homepage dopo la creazione dell'admin
                    return RedirectToAction("Index", "Home");
                }
                catch (Exception)
                {
                    // Gestisce eventuali errori durante la creazione dell'admin
                    ModelState.AddModelError("", "Errore durante la creazione dell'admin. Riprova.");
                }
            }

            // Se il ModelState non è valido, ritorna alla vista CreateAdmin con gli errori
            return View(model);
        }
    }
}
