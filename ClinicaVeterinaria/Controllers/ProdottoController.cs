using ClinicaVeterinaria.Interface;
using ClinicaVeterinaria.Models;
using ClinicaVeterinaria.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClinicaVeterinaria.Controllers
{
    [Authorize(Roles = "Admin,Farmacista")]
    public class ProdottoController : Controller
    {
        private readonly IProdottoService _prodottoService;
        private readonly ILogger<ProdottoController> _logger;

        public ProdottoController(IProdottoService prodottoService, ILogger<ProdottoController> logger)
        {
            _prodottoService = prodottoService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            _logger.LogInformation("Richiesta GET per la creazione di un nuovo prodotto.");
            var viewModel = new ProdottoViewModel
            {
                CaseFarmaceutiche = await _prodottoService.GetCaseFarmaceuticheAsync()
            };

            _logger.LogInformation("Vista Create caricata con successo.");
            return View("~/Views/Prodotti/Create.cshtml", viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProdottoViewModel viewModel)
        {
            _logger.LogInformation("Richiesta POST per la creazione di un nuovo prodotto.");

            // Seleziona la casa farmaceutica esistente
            if (viewModel.SelectedCasaFarmaceuticaId.HasValue)
            {
                // Rimuovi manualmente gli errori di validazione relativi ai campi della nuova casa farmaceutica
                ModelState.Remove(nameof(viewModel.NuovaCasaNome));
                ModelState.Remove(nameof(viewModel.NuovaCasaIndirizzo));
                ModelState.Remove(nameof(viewModel.NuovaCasaEmail));
            }
            else
            {
                // Se non è stata selezionata una casa farmaceutica esistente, i campi per la nuova casa farmaceutica sono obbligatori
                if (string.IsNullOrEmpty(viewModel.NuovaCasaNome) || string.IsNullOrEmpty(viewModel.NuovaCasaIndirizzo) || string.IsNullOrEmpty(viewModel.NuovaCasaEmail))
                {
                    _logger.LogWarning("ModelState non valido. I campi della nuova casa farmaceutica non sono stati compilati.");
                    ModelState.AddModelError("", "Compila i campi della nuova casa farmaceutica o selezionane una esistente.");
                }
            }

            if (ModelState.IsValid)
            {
                if (!viewModel.SelectedCasaFarmaceuticaId.HasValue)
                {
                    var nuovaCasa = new CasaFarmaceutica
                    {
                        Nome = viewModel.NuovaCasaNome,
                        Indirizzo = viewModel.NuovaCasaIndirizzo,
                        Email = viewModel.NuovaCasaEmail
                    };

                    viewModel.SelectedCasaFarmaceuticaId = await _prodottoService.AddCasaFarmaceuticaAsync(nuovaCasa);
                    _logger.LogInformation("Nuova casa farmaceutica creata con successo.");
                }

                await _prodottoService.AddProdottoAsync(viewModel);
                _logger.LogInformation("Prodotto aggiunto con successo.");
                return RedirectToAction("Index", "Home");
            }

            viewModel.CaseFarmaceutiche = await _prodottoService.GetCaseFarmaceuticheAsync();
            _logger.LogWarning("ModelState non valido. Ricaricamento della vista Create.");
            return View("~/Views/Prodotti/Create.cshtml", viewModel);
        }


        // Metodo per visualizzare tutti i prodotti
        [HttpGet]
        public async Task<IActionResult> IndexProdotti()
        {
            _logger.LogInformation("Richiesta GET per la visualizzazione di tutti i prodotti.");
            var prodotti = await _prodottoService.GetAllProdottiAsync();
            _logger.LogInformation("Prodotti caricati con successo.");
            return View("~/Views/Prodotti/IndexProdotti.cshtml", prodotti);
        }

        // Metodo per visualizzare tutte le case farmaceutiche
        [HttpGet]
        public async Task<IActionResult> IndexCaseFarmaceutiche()
        {
            _logger.LogInformation("Richiesta GET per la visualizzazione di tutte le case farmaceutiche.");
            var caseFarmaceutiche = await _prodottoService.GetCaseFarmaceuticheAsync();
            _logger.LogInformation("Case farmaceutiche caricate con successo.");
            return View("~/Views/Prodotti/IndexCaseFarmaceutiche.cshtml", caseFarmaceutiche);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteProdotto(int id)
        {
            _logger.LogInformation("Richiesta POST per la cancellazione del prodotto con ID {Id}.", id);
            await _prodottoService.DeleteProdottoAsync(id);
            _logger.LogInformation("Prodotto con ID {Id} cancellato con successo.", id);
            return RedirectToAction("IndexProdotti");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteCasaFarmaceutica(int id)
        {
            _logger.LogInformation("Richiesta POST per la cancellazione della casa farmaceutica con ID {Id}.", id);
            await _prodottoService.DeleteCasaFarmaceuticaAsync(id);
            _logger.LogInformation("Casa farmaceutica con ID {Id} cancellata con successo.", id);
            return RedirectToAction("IndexCaseFarmaceutiche");
        }
    }
}
