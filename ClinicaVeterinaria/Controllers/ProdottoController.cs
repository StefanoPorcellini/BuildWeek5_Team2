using ClinicaVeterinaria.Interface;
using ClinicaVeterinaria.Models;
using ClinicaVeterinaria.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ClinicaVeterinaria.Controllers
{
    [Authorize(Roles = "Admin,Farmacista")]
    public class ProdottoController : Controller
    {
        private readonly IProdottoService _prodottoService;
        private readonly ILogger<ProdottoController> _logger; // Aggiungi il logger

        // Costruttore che inietta i servizi necessari
        public ProdottoController(IProdottoService prodottoService, ILogger<ProdottoController> logger)
        {
            _prodottoService = prodottoService;
            _logger = logger; // Inizializza il logger
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

            // Assicurati che ViewBag.CaseFarmaceutiche sia popolato
            ViewBag.CaseFarmaceutiche = await _prodottoService.GetCaseFarmaceuticheAsync();

            var viewModel = new EditProdottoViewModel
            {
                Id = prodotto.Id,
                Nome = prodotto.Nome,
                Prezzo = prodotto.Prezzo,
                NumeroArmadietto = prodotto.NumeroArmadietto,
                NumeroCassetto = prodotto.NumeroCassetto,
                Tipologia = prodotto.Tipologia,
                CasaFarmaceuticaId = prodotto.CasaFarmaceuticaId,
                CaseFarmaceutiche = ViewBag.CaseFarmaceutiche // Inizializza il ViewBag qui
            };

            return View("~/Views/Prodotti/EditProdotto.cshtml", viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> EditProdotto(EditProdottoViewModel viewModel)
        {
            _logger.LogInformation("Inizio del metodo POST per la modifica del prodotto con ID {Id}", viewModel.Id);

            // Rimuovi eventuali campi non necessari dal ModelState
            ModelState.Remove("CaseFarmaceutiche");

            if (ModelState.IsValid)
            {
                _logger.LogInformation("ModelState valido per la modifica del prodotto con ID {Id}", viewModel.Id);

                var prodotto = await _prodottoService.GetProdottoByIdAsync(viewModel.Id);

                if (prodotto == null)
                {
                    _logger.LogWarning("Prodotto con ID {Id} non trovato", viewModel.Id);
                    return NotFound();
                }

                _logger.LogInformation("Prodotto con ID {Id} trovato", viewModel.Id);

                // Aggiorna i campi del prodotto
                prodotto.Nome = viewModel.Nome;
                prodotto.Prezzo = viewModel.Prezzo;
                prodotto.NumeroArmadietto = viewModel.NumeroArmadietto;
                prodotto.NumeroCassetto = viewModel.NumeroCassetto;
                prodotto.Tipologia = viewModel.Tipologia;
                prodotto.CasaFarmaceuticaId = viewModel.CasaFarmaceuticaId;

                try
                {
                    _logger.LogInformation("Tentativo di aggiornamento del prodotto con ID {Id}", viewModel.Id);
                    await _prodottoService.UpdateProdottoAsync(prodotto);
                    _logger.LogInformation("Prodotto con ID {Id} aggiornato con successo", viewModel.Id);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Errore durante l'aggiornamento del prodotto con ID {Id}", viewModel.Id);
                    ModelState.AddModelError("", "Errore durante l'aggiornamento del prodotto: " + ex.Message);
                    ViewBag.CaseFarmaceutiche = await _prodottoService.GetCaseFarmaceuticheAsync();
                    return View("~/Views/Prodotti/EditProdotto.cshtml", viewModel);
                }

                return RedirectToAction("IndexProdotti");
            }
            else
            {
                _logger.LogWarning("ModelState non valido per la modifica del prodotto con ID {Id}", viewModel.Id);
            }

            // Se il ModelState non è valido, ripopola ViewBag.CaseFarmaceutiche e torna alla vista di modifica
            ViewBag.CaseFarmaceutiche = await _prodottoService.GetCaseFarmaceuticheAsync();
            return View("~/Views/Prodotti/EditProdotto.cshtml", viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> EditCasaFarmaceutica(int id)
        {
            _logger.LogInformation("Richiesta GET per la modifica della casa farmaceutica con ID {Id}", id);

            // Recupera la casa farmaceutica dal database
            var casaFarmaceutica = await _prodottoService.GetCasaFarmaceuticaByIdAsync(id);
            if (casaFarmaceutica == null)
            {
                _logger.LogWarning("Casa farmaceutica con ID {Id} non trovata", id);
                return NotFound();
            }

            // Popola il ViewModel con i dati della casa farmaceutica esistente
            var viewModel = new EditCasaFarmaceuticaViewModel
            {
                Id = casaFarmaceutica.Id,
                Nome = casaFarmaceutica.Nome,
                Indirizzo = casaFarmaceutica.Indirizzo,
                Email = casaFarmaceutica.Email
            };

            _logger.LogInformation("Vista di modifica della casa farmaceutica con ID {Id} caricata con successo", id);

            return View("~/Views/Prodotti/EditCasaFarmaceutica.cshtml", viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> EditCasaFarmaceutica(EditCasaFarmaceuticaViewModel viewModel)
        {
            _logger.LogInformation("Inizio del metodo POST per la modifica della casa farmaceutica con ID {Id}", viewModel.Id);

            if (ModelState.IsValid)
            {
                _logger.LogInformation("ModelState valido per la modifica della casa farmaceutica con ID {Id}", viewModel.Id);

                // Recupera la casa farmaceutica esistente dal database
                var casaFarmaceutica = await _prodottoService.GetCasaFarmaceuticaByIdAsync(viewModel.Id);
                if (casaFarmaceutica == null)
                {
                    _logger.LogWarning("Casa farmaceutica con ID {Id} non trovata", viewModel.Id);
                    return NotFound();
                }

                // Aggiorna i campi della casa farmaceutica
                casaFarmaceutica.Nome = viewModel.Nome;
                casaFarmaceutica.Indirizzo = viewModel.Indirizzo;
                casaFarmaceutica.Email = viewModel.Email;

                try
                {
                    _logger.LogInformation("Tentativo di aggiornamento della casa farmaceutica con ID {Id}", viewModel.Id);

                    // Salva le modifiche nel database
                    await _prodottoService.UpdateCasaFarmaceuticaAsync(casaFarmaceutica);

                    _logger.LogInformation("Casa farmaceutica con ID {Id} aggiornata con successo", viewModel.Id);

                    return RedirectToAction("IndexCaseFarmaceutiche");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Errore durante l'aggiornamento della casa farmaceutica con ID {Id}", viewModel.Id);
                    ModelState.AddModelError("", "Errore durante l'aggiornamento della casa farmaceutica: " + ex.Message);
                    return View("~/Views/Prodotti/EditCasaFarmaceutica.cshtml", viewModel);
                }
            }
            else
            {
                _logger.LogWarning("ModelState non valido per la modifica della casa farmaceutica con ID {Id}", viewModel.Id);
            }

            // Se il ModelState non è valido, torna alla vista di modifica
            return View("~/Views/Prodotti/EditCasaFarmaceutica.cshtml", viewModel);
        }
        [HttpGet]
        public IActionResult RicercaProdotti(string query)
        {
            // Simula il recupero dei prodotti dalla memoria
            var prodotti = _prodottoService.GetProdottiMemoria();

            // Se la query è null o vuota, restituisci tutti i prodotti oppure un elenco vuoto.
            if (string.IsNullOrEmpty(query))
            {
                // Restituisci un elenco vuoto per evitare l'errore.
                return Json(new List<object>());
            }

            // Filtra i prodotti in base alla query
            var risultati = prodotti
                .Where(p => !string.IsNullOrEmpty(p.Nome) && p.Nome.Contains(query, StringComparison.OrdinalIgnoreCase))
                .Select(p => new { p.Nome, p.NumeroArmadietto, p.NumeroCassetto })
                .ToList();

            return Json(risultati);
        }

        [HttpGet]
        public IActionResult RicercaProdotto()
        {
            // Specifichiamo esplicitamente il percorso completo della vista, che si trova nella cartella "Prodotti"
            return View("~/Views/Prodotti/RicercaProdotto.cshtml");
        }


        [HttpPost]
        public IActionResult CercaProdotto(string query)
        {
            // Simula il recupero dei prodotti dalla memoria (in un contesto reale, questo potrebbe essere una cache o una lista già caricata)
            var prodotti = _prodottoService.GetProdottiMemoria();

            // Se la query è null o vuota, restituisci tutti i prodotti o un elenco vuoto
            if (string.IsNullOrEmpty(query))
            {
                // Restituisci un elenco vuoto per evitare l'errore, oppure restituisci tutti i prodotti.
                return Json(new List<object>());
            }

            // Filtra i prodotti in base alla query, ignorando i prodotti con Nome null
            var risultatiFiltrati = prodotti
                .Where(p => !string.IsNullOrEmpty(p.Nome) && p.Nome.Contains(query, StringComparison.OrdinalIgnoreCase))
                .Select(p => new { p.Nome, p.NumeroArmadietto, p.NumeroCassetto })
                .ToList();

            return Json(risultatiFiltrati);
        }

    }
}
