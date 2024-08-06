using ClinicaVeterinaria.Intertface;
using ClinicaVeterinaria.Models;
using ClinicaVeterinaria.Service;
using Microsoft.AspNetCore.Mvc;

namespace ClinicaVeterinaria.Controllers
{
    [Route("User")]
    public class UtenteController : Controller
    {
        private readonly IUtenteService _utenteService;
        private readonly IAuthenticationService _authenticationService;

        public UtenteController(IUtenteService utenteService, IAuthenticationService authenticationService)
        {
            _utenteService = utenteService;
            _authenticationService = authenticationService;
        }

        [HttpGet("Login")]
        public IActionResult Login()
        {
            return View("~/Views/User/Login.cshtml");
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(string username, string password)
        {
            // Tentativo di login dell'utente
            var user = await _utenteService.LoginAsync(username, password);
            if (user != null)
            {
                // Esegui il login tramite il servizio di autenticazione
                await _authenticationService.SignInAsync(user);
                return RedirectToAction("Index", "Home");
            }

            // Se le credenziali sono errate, mostra un messaggio di errore
            ModelState.AddModelError(string.Empty, "Username o password errati.");
            return View("~/Views/User/Login.cshtml");
        }

        [HttpGet("Logout")]
        public async Task<IActionResult> Logout()
        {
            // Esegui il logout tramite il servizio di autenticazione
            await _authenticationService.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet("Register")]
        public IActionResult Register()
        {
            return View("~/Views/User/Register.cshtml");
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(Utente newUser, string password)
        {
            // Tentativo di registrazione di un nuovo utente
            if (ModelState.IsValid)
            {
                try
                {
                    // Imposta la password e salva l'utente
                    newUser.SetPassword(password);
                    await _utenteService.CreateUtenteAsync(newUser);
                    return RedirectToAction("Login", "Utente");
                }
                catch
                {
                    ModelState.AddModelError("", "Errore durante la registrazione. Riprova.");
                }
            }

            return View("~/Views/User/Register.cshtml", newUser);
        }

        [HttpGet("AccessDenied")]
        public IActionResult AccessDenied()
        {
            return View("~/Views/User/AccessDenied.cshtml");
        }

        [HttpGet("CreateAdmin")]
        public IActionResult CreateAdmin()
        {
            return View("~/Views/User/CreateAdmin.cshtml");
        }

        [HttpPost("CreateAdmin")]
        public async Task<IActionResult> CreateAdmin(Utente newAdmin, string password, string ruolo)
        {
            // Tentativo di creazione di un nuovo admin
            newAdmin.SetPassword(password);
            newAdmin.Ruolo = ruolo;

            // Cancella lo stato del modello per ricalcolarlo
            ModelState.Clear();

            // Ora valida il modello
            if (TryValidateModel(newAdmin))
            {
                try
                {
                    await _utenteService.CreateUtenteAsync(newAdmin);
                    return RedirectToAction("Index", "Home");
                }
                catch
                {
                    ModelState.AddModelError("", "Errore durante la creazione dell'admin. Riprova.");
                }
            }

            return View("~/Views/User/CreateAdmin.cshtml", newAdmin);
        }
    }
}
