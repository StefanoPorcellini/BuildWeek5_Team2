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
    public async Task<IActionResult> Create([Bind("Id,Nome,TipologiaAnimale,ColoreManto,DataNascita,PossiedeChip,NumeroChip,ProprietarioId,Randagio")] Animale animale, IFormFile? img)
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

                // Salva l'animale senza l'immagine
                await _animaleService.CreateAsync(animale);

                if (img != null && img.Length > 0)
                {
                    // Salva l'immagine sul disco
                    _animaleService.SaveImg(animale.Id, img);

                    // Aggiorna il percorso dell'immagine nel modello
                    animale.Foto = $"foto/fotoAnimale{animale.Id}.jpg";

                    // Aggiorna l'animale nel database con il percorso dell'immagine
                    await _animaleService.UpdateAsync(animale);
                }

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

    // POST: Animali/Search
    [HttpPost]
    public async Task<IActionResult> SearchAnimal(string chipNumber)
    {
        TempData["SearchInit"] = true;
        var animal = await _animaleService.SearchByChipNumberAsync(chipNumber);

        if (animal != null)
        {
            TempData["AnimalFound"] = true;
            TempData["AnimalName"] = animal.Nome;
            TempData["AnimalType"] = animal.TipologiaAnimale;
            TempData["AnimalColor"] = animal.ColoreManto;
            TempData["AnimalBirthDate"] = animal.DataNascita?.ToString("dd/MM/yyyy");
            TempData["AnimalChipNumber"] = animal.NumeroChip;
        }
        else
        {
            TempData["AnimalFound"] = false;
        }

        return RedirectToAction("Index", "Home");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateWithProprietario(
    [Bind("Id,Nome,TipologiaAnimale,ColoreManto,DataNascita,PossiedeChip,NumeroChip,Randagio")] Animale animale,
    IFormFile? img,
    int? ProprietarioId)
    {
        if (ModelState.IsValid)
        {
            // Verifica se è stato selezionato un proprietario esistente
            if (ProprietarioId.HasValue)
            {
                _logger.LogInformation("Proprietario esistente selezionato, ID: {ProprietarioId}", ProprietarioId.Value);

                // Assegna l'ID del proprietario all'animale
                animale.ProprietarioId = ProprietarioId.Value;

                // Salva l'animale nel database
                await _animaleService.CreateAsync(animale);

                // Salva l'immagine se presente
                if (img != null && img.Length > 0)
                {
                    _animaleService.SaveImg(animale.Id, img);
                    animale.Foto = $"foto/fotoAnimale{animale.Id}.jpg";
                    await _animaleService.UpdateAsync(animale);
                }
            }
            else
            {
                ModelState.AddModelError("", "Devi selezionare un proprietario.");
                _logger.LogError("Proprietario non selezionato.");
                return View(animale);
            }

            // Reindirizza alla pagina Index
            return RedirectToAction(nameof(Index));
        }

        // Se il ModelState non è valido, ritorna la vista con gli errori
        _logger.LogError("ModelState non valido, ritorno alla vista.");
        return View(animale);
    }
}
