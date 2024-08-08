using ClinicaVeterinaria.Models;
using ClinicaVeterinaria.Service.Intertface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

[Authorize(Roles = "Veterinario,Admin")]
public class ProprietariController : Controller
{
    private readonly IProprietarioService _proprietarioService;
    private readonly ILogger<ProprietariController> _logger;

    public ProprietariController(IProprietarioService proprietarioService, ILogger<ProprietariController> logger)
    {
        _proprietarioService = proprietarioService;
        _logger = logger;
    }   

    public async Task<IActionResult> Index()
    {
        var proprietari = await _proprietarioService.GetAllAsync();
        return View(proprietari);
    }

    public async Task<IActionResult> Details(int id)
    {
        var proprietario = await _proprietarioService.GetByIdAsync(id);
        if (proprietario == null)
        {
            return NotFound();
        }
        return View(proprietario);
    }

    // GET: Proprietari/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Proprietari/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Nome,Cognome,Telefono,Indirizzo,Citta,CodiceFiscale")] Proprietario proprietario)
    {
        if (ModelState.IsValid)
        {
            await _proprietarioService.CreateAsync(proprietario);
            return RedirectToAction(nameof(Index));
        }
        return View(proprietario);
    }

    // GET: Proprietari/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        var proprietario = await _proprietarioService.GetByIdAsync(id);
        if (proprietario == null)
        {
            return NotFound();
        }
        return View(proprietario);
    }

    // POST: Proprietari/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Nome,Cognome,Telefono,Indirizzo,Citta,CodiceFiscale")] Proprietario proprietario)
    {
        if (id != proprietario.Id)
        {
            _logger.LogWarning("L'ID fornito ({Id}) non corrisponde all'ID del modello ({ProprietarioId}).", id, proprietario.Id);
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                await _proprietarioService.UpdateAsync(proprietario);
                _logger.LogInformation("Proprietario con ID {Id} aggiornato con successo.", proprietario.Id);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (await _proprietarioService.GetByIdAsync(proprietario.Id) == null)
                {
                    _logger.LogWarning("Proprietario con ID {Id} non trovato durante il tentativo di aggiornamento.", proprietario.Id);
                    return NotFound();
                }
                else
                {
                    _logger.LogError(ex, "Errore di concorrenza durante l'aggiornamento del proprietario con ID {Id}.", proprietario.Id);
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }
        else
        {
            _logger.LogWarning("ModelState non valido per il proprietario con ID {Id}.", proprietario.Id);

            // Itera sugli errori del ModelState
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

        return View(proprietario);
    }

    // GET: Proprietari/Delete/5
    public async Task<IActionResult> Delete(int id)
    {
        var proprietario = await _proprietarioService.GetByIdAsync(id);
        if (proprietario == null)
        {
            return NotFound();
        }

        return View(proprietario); // Mostra la vista di conferma dell'eliminazione
    }

    // POST: Proprietari/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var proprietario = await _proprietarioService.GetByIdAsync(id);
        if (proprietario == null)
        {
            return NotFound();
        }

        await _proprietarioService.DeleteAsync(id);
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Search(string term)
    {
        if (string.IsNullOrWhiteSpace(term))
        {
            return Json(new { success = false, message = "Termine di ricerca non valido." });
        }

        var proprietari = await _proprietarioService.SearchAsync(term);

        var result = proprietari.Select(p => new
        {
            p.Id,
            p.Nome,
            p.Cognome,
            p.Telefono,
            p.Indirizzo,
            p.Citta,
            p.CodiceFiscale
        }).ToList();

        return Json(result);
    }
    [HttpGet]
    public async Task<IActionResult> GetAllProprietari()
    {
        var proprietari = await _proprietarioService.GetAllAsync();
        return Json(proprietari);
    }
}
