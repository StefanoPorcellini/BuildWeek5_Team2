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
            if (ModelState.IsValid)
            {
                if (!viewModel.SelectedCasaFarmaceuticaId.HasValue)
                {
                    if (!string.IsNullOrEmpty(viewModel.NuovaCasaNome))
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
                    else
                    {
                        _logger.LogWarning("ModelState non valido. I campi della nuova casa farmaceutica non sono stati compilati.");
                        ModelState.AddModelError("", "Compila i campi della nuova casa farmaceutica o selezionane una esistente.");
                        viewModel.CaseFarmaceutiche = await _prodottoService.GetCaseFarmaceuticheAsync();
                        return View("~/Views/Prodotti/Create.cshtml", viewModel);
                    }
                }

                await _prodottoService.AddProdottoAsync(viewModel);
                _logger.LogInformation("Prodotto aggiunto con successo.");
                return RedirectToAction("Index", "Home");
            }

            viewModel.CaseFarmaceutiche = await _prodottoService.GetCaseFarmaceuticheAsync();
            _logger.LogWarning("ModelState non valido. Ricaricamento della vista Create.");
            return View("~/Views/Prodotti/Create.cshtml", viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> IndexProdotti()
        {
            var prodotti = await _prodottoService.GetAllProdottiAsync();
            return View("~/Views/Prodotti/IndexProdotti.cshtml", prodotti);
        }

        [HttpGet]
        public async Task<IActionResult> IndexCaseFarmaceutiche()
        {
            var caseFarmaceutiche = await _prodottoService.GetCaseFarmaceuticheAsync();
            return View("~/Views/Prodotti/IndexCaseFarmaceutiche.cshtml", caseFarmaceutiche);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteProdotto(int id)
        {
            await _prodottoService.DeleteProdottoAsync(id);
            return RedirectToAction("IndexProdotti");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteCasaFarmaceutica(int id)
        {
            await _prodottoService.DeleteCasaFarmaceuticaAsync(id);
            return RedirectToAction("IndexCaseFarmaceutiche");
        }
    }
}
