using ClinicaVeterinaria.Models;
using ClinicaVeterinaria.Service.Intertface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

public class AnimaliController : Controller
{
    private readonly IAnimaleService _animaleService;
    private readonly IProprietarioService _proprietarioService;
    private readonly ILogger<AnimaliController> _logger;

    public AnimaliController(IAnimaleService animaleService, IProprietarioService proprietarioService, ILogger<AnimaliController> logger)
    {
        _animaleService = animaleService;
        _proprietarioService = proprietarioService;
        _logger = logger;
    }

    // GET: Animali
    public async Task<IActionResult> Index()
    {
        var animali = await _animaleService.GetAllAsync();
        return View(animali);
    }



    // GET: Animali/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var animale = await _animaleService.GetByIdAsync(id.Value);
        if (animale == null)
        {
            return NotFound();
        }

        return View(animale);
    }

    // GET: Animali/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Animali/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Nome,TipologiaAnimale,ColoreManto,DataNascita,PossiedeChip,NumeroChip,ProprietarioId,Randagio")] Animale animale)
    {
        _logger.LogInformation("Avviata la creazione di un nuovo animale.");

        // Verifica se l'animale è randagio e non ha un proprietario
        if (!animale.Randagio && !animale.ProprietarioId.HasValue)
        {
            _logger.LogWarning("ProprietarioId è nullo. Non è possibile creare l'animale non randagio senza un proprietario associato.");
            ModelState.AddModelError("ProprietarioId", "È necessario selezionare un proprietario per un animale non randagio.");
        }

        if (ModelState.IsValid)
        {
            try
            {
                _logger.LogInformation("Modello valido. Inizio del processo di creazione per l'animale con Nome: {Nome}, Tipologia: {Tipologia}, Randagio: {Randagio}.",
                    animale.Nome, animale.TipologiaAnimale, animale.Randagio);

                if (!animale.Randagio && animale.ProprietarioId.HasValue)
                {
                    // Carica il proprietario associato solo se l'animale non è randagio
                    animale.Proprietario = await _proprietarioService.GetByIdAsync(animale.ProprietarioId.Value);
                }

                await _animaleService.CreateAsync(animale);
                _logger.LogInformation("Animale creato con successo. Nome: {Nome}, ID: {Id}.", animale.Nome, animale.Id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Errore durante la creazione dell'animale con Nome: {Nome}, Tipologia: {Tipologia}, Randagio: {Randagio}.",
                    animale.Nome, animale.TipologiaAnimale, animale.Randagio);
                ModelState.AddModelError(string.Empty, "Si è verificato un errore durante la creazione dell'animale. Riprova più tardi.");
            }
        }
        else
        {
            _logger.LogWarning("Modello non valido. Creazione dell'animale fallita. Dettagli degli errori:");

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

        return View(animale);
    }

    // GET: Animali/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var animale = await _animaleService.GetByIdAsync(id.Value);
        if (animale == null)
        {
            return NotFound();
        }
        return View(animale);
    }

    // POST: Animali/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Nome,TipologiaAnimale,ColoreManto,DataNascita,PossiedeChip,NumeroChip,ProprietarioId,Randagio")] Animale animale)
    {
        if (id != animale.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                await _animaleService.UpdateAsync(animale);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_animaleService.AnimaleExists(animale.Id))
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
        return View(animale);
    }

    // GET: Animali/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var animale = await _animaleService.GetByIdAsync(id.Value);
        if (animale == null)
        {
            return NotFound();
        }

        return View(animale);
    }

    // POST: Animali/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _animaleService.DeleteAsync(id);
        return RedirectToAction(nameof(Index));
    }
}
