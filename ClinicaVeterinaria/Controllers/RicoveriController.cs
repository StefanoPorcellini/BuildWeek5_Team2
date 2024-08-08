using ClinicaVeterinaria.Interface;
using ClinicaVeterinaria.Models;
using ClinicaVeterinaria.Models.Requests;
using ClinicaVeterinaria.Service.Intertface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

[Authorize(Roles = "Veterinario,Admin")]
public class RicoveriController : Controller
{
    private readonly IRicoveroService _ricoveroService;
    private readonly IAnimaleService _animaleService;
    private readonly ILogger<RicoveriController> _logger;

    public RicoveriController(IRicoveroService ricoveroService, IAnimaleService animaleService, ILogger<RicoveriController> logger)
    {
        _ricoveroService = ricoveroService;
        _animaleService = animaleService;
        _logger = logger;
    }


    // GET: Ricoveri
    public async Task<IActionResult> Index()
    {
        var ricoveri = await _ricoveroService.GetAllAsync();
        return View(ricoveri);
    }

    // GET: Ricoveri/Details/5
    public async Task<IActionResult> Details(int id)
    {
        var ricovero = await _ricoveroService.GetByIdAsync(id);
        if (ricovero == null)
        {
            return NotFound();
        }
        return View(ricovero);
    }

    // GET: Ricoveri/Create
    public async Task<IActionResult> Create()
    {
        // Ottenere l'elenco di animali dal database tramite il servizio
        var animali = await _animaleService.GetAllAsync();

        // Creare un nuovo oggetto Ricovero con DataInizio impostato a DateTime.Now
        var ricovero = new Ricovero
        {
            DataInizio = DateTime.Now
        };

        // Popolare ViewBag con l'elenco di animali per il dropdown nella vista
        ViewBag.AnimaleId = new SelectList(animali, "Id", "Nome");

        return View(ricovero);
    }

    // POST: Ricoveri/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,DataInizio,DataFine,AnimaleId,Rimborso,CostoGiornaliero")] Ricovero ricovero)
    {
        _logger.LogInformation("Inizio processo di creazione di un nuovo ricovero.");

        // Popola l'oggetto Animale in base all'AnimaleId
        if (ricovero.AnimaleId > 0)
        {
            var animale = await _animaleService.GetByIdAsync(ricovero.AnimaleId);
            if (animale != null)
            {
                ricovero.Animale = animale;
                _logger.LogInformation("Animale popolato con successo per l'ID: {AnimaleId}", ricovero.AnimaleId);
            }
            else
            {
                _logger.LogWarning("Animale con ID: {AnimaleId} non trovato.", ricovero.AnimaleId);
                ModelState.AddModelError("AnimaleId", "Animale non trovato.");
            }
        }

        if (ModelState.IsValid)
        {
            try
            {
                _logger.LogInformation("Modello valido. Inizio creazione del ricovero per l'animale con ID: {AnimaleId}, Data Inizio: {DataInizio}", ricovero.AnimaleId, ricovero.DataInizio);

                await _ricoveroService.CreateAsync(ricovero);

                _logger.LogInformation("Ricovero creato con successo. ID Ricovero: {RicoveroId}", ricovero.Id);

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Errore durante la creazione del ricovero per l'animale con ID: {AnimaleId}", ricovero.AnimaleId);
                ModelState.AddModelError(string.Empty, "Si è verificato un errore durante la creazione del ricovero. Riprova più tardi.");
            }
        }
        else
        {
            _logger.LogWarning("Modello non valido. Dettagli degli errori:");

            foreach (var state in ModelState)
            {
                var key = state.Key;
                var errors = state.Value.Errors;

                foreach (var error in errors)
                {
                    _logger.LogWarning("Errore nel campo '{Field}': {ErrorMessage}", key, error.ErrorMessage);
                }
            }
        }

        _logger.LogInformation("Fine processo di creazione del ricovero con modello non valido.");
        return View(ricovero);
    }



    // GET: Ricoveri/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        var ricovero = await _ricoveroService.GetByIdAsync(id);
        if (ricovero == null)
        {
            return NotFound();
        }

        var animali = await _animaleService.GetAllAsync();
        ViewBag.AnimaleId = new SelectList(animali, "Id", "Nome", ricovero.AnimaleId);

        return View(ricovero);
    }

    // POST: Ricoveri/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,DataInizio,DataFine,AnimaleId,Rimborso,CostoGiornaliero")] Ricovero ricovero)
    {
        if (id != ricovero.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                await _ricoveroService.UpdateAsync(ricovero);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await RicoveroExists(ricovero.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }
        return View(ricovero);
    }

    // GET: Ricoveri/Delete/5
    public async Task<IActionResult> Delete(int id)
    {
        var ricovero = await _ricoveroService.GetByIdAsync(id);
        if (ricovero == null)
        {
            return NotFound();
        }
        return View(ricovero);
    }

    // POST: Ricoveri/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _ricoveroService.DeleteAsync(id);
        return RedirectToAction(nameof(Index));
    }

    // GET: Ricoveri/Checkout/5
    public async Task<IActionResult> Checkout(int id)
    {
        _logger.LogInformation("Avviato il processo di checkout per il ricovero con ID: {RicoveroId}", id);

        var ricovero = await _ricoveroService.GetRicoveroWithAnimaleAsync(id);
        if (ricovero == null)
        {
            _logger.LogWarning("Ricovero con ID: {RicoveroId} non trovato.", id);
            return NotFound();
        }

        // Qui non calcoliamo ancora il costo totale, lasciamo che l'utente selezioni la data di fine

        _logger.LogInformation("Ricovero trovato per l'ID: {RicoveroId}. Pronto per il checkout.", id);

        return View(ricovero);
    }



    // POST: Ricoveri/ConfirmCheckout/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ConfirmCheckout(int id, DateTime? dataFine)
    {
        _logger.LogInformation("Avviato il processo di conferma checkout per il ricovero con ID: {RicoveroId}", id);

        var ricovero = await _ricoveroService.GetByIdAsync(id);
        if (ricovero == null)
        {
            _logger.LogWarning("Ricovero con ID: {RicoveroId} non trovato.", id);
            return NotFound();
        }

        if (dataFine.HasValue)
        {
            ricovero.DataFine = dataFine.Value;
            _logger.LogInformation("Data di fine aggiornata a: {DataFine}", dataFine.Value);
        }
        else
        {
            _logger.LogWarning("Data di fine non fornita, verrà utilizzata la data odierna.");
            ricovero.DataFine = DateTime.Now;
        }

        // Calcola il costo totale passando la data di fine aggiornata
        var costoTotale = await _ricoveroService.CalcolaCostoTotaleAsync(ricovero.Id, ricovero.DataFine);
        ricovero.PrezzoTotale = costoTotale;

        await _ricoveroService.UpdateAsync(ricovero);
        _logger.LogInformation("Ricovero con ID: {RicoveroId} aggiornato con successo.", id);

        return RedirectToAction(nameof(Index));
    }







    private async Task<bool> RicoveroExists(int id)
    {
        var ricovero = await _ricoveroService.GetByIdAsync(id);
        return ricovero != null;
    }
}
