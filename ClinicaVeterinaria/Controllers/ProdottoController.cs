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

        // Costruttore che inietta i servizi necessari
        public ProdottoController(IProdottoService prodottoService)
        {
            _prodottoService = prodottoService;
        }

        // Metodo per gestire la richiesta GET per la creazione di un nuovo prodotto
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            // Carica le case farmaceutiche esistenti e prepara il ViewModel per la vista
            var viewModel = new ProdottoViewModel
            {
                CaseFarmaceutiche = await _prodottoService.GetCaseFarmaceuticheAsync()
            };

            return View("~/Views/Prodotti/Create.cshtml", viewModel);
        }

        // Metodo per gestire la richiesta POST per la creazione di un nuovo prodotto
        [HttpPost]
        public async Task<IActionResult> Create(ProdottoViewModel viewModel)
        {
            // Se è stata selezionata una casa farmaceutica esistente, rimuovi gli errori di validazione per i campi della nuova casa farmaceutica
            if (viewModel.SelectedCasaFarmaceuticaId.HasValue)
            {
                ModelState.Remove(nameof(viewModel.NuovaCasaNome));
                ModelState.Remove(nameof(viewModel.NuovaCasaIndirizzo));
                ModelState.Remove(nameof(viewModel.NuovaCasaEmail));
            }
            else
            {
                // Se non è stata selezionata una casa farmaceutica esistente, i campi per la nuova casa farmaceutica devono essere compilati
                if (string.IsNullOrEmpty(viewModel.NuovaCasaNome) || string.IsNullOrEmpty(viewModel.NuovaCasaIndirizzo) || string.IsNullOrEmpty(viewModel.NuovaCasaEmail))
                {
                    ModelState.AddModelError("", "Compila i campi della nuova casa farmaceutica o selezionane una esistente.");
                }
            }

            // Se lo stato del ModelState è valido, procedi con la creazione del prodotto
            if (ModelState.IsValid)
            {
                // Se non è stata selezionata una casa farmaceutica esistente, crea una nuova casa farmaceutica
                if (!viewModel.SelectedCasaFarmaceuticaId.HasValue)
                {
                    var nuovaCasa = new CasaFarmaceutica
                    {
                        Nome = viewModel.NuovaCasaNome,
                        Indirizzo = viewModel.NuovaCasaIndirizzo,
                        Email = viewModel.NuovaCasaEmail
                    };

                    viewModel.SelectedCasaFarmaceuticaId = await _prodottoService.AddCasaFarmaceuticaAsync(nuovaCasa);
                }

                // Aggiungi il nuovo prodotto
                await _prodottoService.AddProdottoAsync(viewModel);
                return RedirectToAction("Index", "Home");
            }

            // Se il ModelState non è valido, ricarica la lista delle case farmaceutiche e torna alla vista Create
            viewModel.CaseFarmaceutiche = await _prodottoService.GetCaseFarmaceuticheAsync();
            return View("~/Views/Prodotti/Create.cshtml", viewModel);
        }

        // Metodo per visualizzare tutti i prodotti
        [HttpGet]
        public async Task<IActionResult> IndexProdotti()
        {
            var prodotti = await _prodottoService.GetAllProdottiAsync();
            return View("~/Views/Prodotti/IndexProdotti.cshtml", prodotti);
        }

        // Metodo per visualizzare tutte le case farmaceutiche
        [HttpGet]
        public async Task<IActionResult> IndexCaseFarmaceutiche()
        {
            var caseFarmaceutiche = await _prodottoService.GetCaseFarmaceuticheAsync();
            return View("~/Views/Prodotti/IndexCaseFarmaceutiche.cshtml", caseFarmaceutiche);
        }

        // Metodo per eliminare un prodotto
        [HttpPost]
        public async Task<IActionResult> DeleteProdotto(int id)
        {
            await _prodottoService.DeleteProdottoAsync(id);
            return RedirectToAction("IndexProdotti");
        }

        // Metodo per eliminare una casa farmaceutica
        [HttpPost]
        public async Task<IActionResult> DeleteCasaFarmaceutica(int id)
        {
            await _prodottoService.DeleteCasaFarmaceuticaAsync(id);
            return RedirectToAction("IndexCaseFarmaceutiche");
        }

        [HttpGet]
        public async Task<IActionResult> EditProdotto(int id)
        {
            var prodotto = await _prodottoService.GetProdottoByIdAsync(id);
            if (prodotto == null)
            {
                return NotFound();
            }

            var viewModel = new EditProdottoViewModel
            {
                Id = prodotto.Id,
                Nome = prodotto.Nome,
                Prezzo = prodotto.Prezzo,
                NumeroArmadietto = prodotto.NumeroArmadietto,
                NumeroCassetto = prodotto.NumeroCassetto,
                Tipologia = prodotto.Tipologia,
                CasaFarmaceuticaId = prodotto.CasaFarmaceuticaId,
                CaseFarmaceutiche = await _prodottoService.GetCaseFarmaceuticheAsync()
            };

            return View("~/Views/Prodotti/EditProdotto.cshtml", viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> EditProdotto(EditProdottoViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var prodotto = new Prodotto
                {
                    Id = viewModel.Id,
                    Nome = viewModel.Nome,
                    Prezzo = viewModel.Prezzo,
                    NumeroArmadietto = viewModel.NumeroArmadietto,
                    NumeroCassetto = viewModel.NumeroCassetto,
                    Tipologia = viewModel.Tipologia,
                    CasaFarmaceuticaId = viewModel.CasaFarmaceuticaId
                };

                await _prodottoService.UpdateProdottoAsync(prodotto);
                return RedirectToAction("IndexProdotti");
            }

            viewModel.CaseFarmaceutiche = await _prodottoService.GetCaseFarmaceuticheAsync();
            return View("~/Views/Prodotti/EditProdotto.cshtml", viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> EditCasaFarmaceutica(int id)
        {
            var casaFarmaceutica = await _prodottoService.GetCasaFarmaceuticaByIdAsync(id);
            if (casaFarmaceutica == null)
            {
                return NotFound();
            }

            var viewModel = new EditCasaFarmaceuticaViewModel
            {
                Id = casaFarmaceutica.Id,
                Nome = casaFarmaceutica.Nome,
                Indirizzo = casaFarmaceutica.Indirizzo,
                Email = casaFarmaceutica.Email
            };

            return View("~/Views/Prodotti/EditCasaFarmaceutica.cshtml", viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> EditCasaFarmaceutica(EditCasaFarmaceuticaViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var casaFarmaceutica = new CasaFarmaceutica
                {
                    Id = viewModel.Id,
                    Nome = viewModel.Nome,
                    Indirizzo = viewModel.Indirizzo,
                    Email = viewModel.Email
                };

                await _prodottoService.UpdateCasaFarmaceuticaAsync(casaFarmaceutica);
                return RedirectToAction("IndexCaseFarmaceutiche");
            }

            return View("~/Views/Prodotti/EditCasaFarmaceutica.cshtml", viewModel);
        }
    }
}
