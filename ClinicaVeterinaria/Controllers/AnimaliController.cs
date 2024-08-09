using ClinicaVeterinaria.Models;
using ClinicaVeterinaria.Service.Intertface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;


[Authorize(Roles = "Veterinario,Admin")]
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

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Nome,TipologiaAnimale,ColoreManto,DataNascita,PossiedeChip,NumeroChip,ProprietarioId,Randagio")] Animale animale, IFormFile? img, string EliminaFoto)
    {
        if (id != animale.Id)
        {
            return NotFound();
        }

        // Rimuovi EliminaFoto dal ModelState
        ModelState.Remove("EliminaFoto");

        if (ModelState.IsValid)
        {
            try
            {
                // Recupera l'animale esistente dal database
                var animaleEsistente = await _animaleService.GetByIdAsync(id);
                if (animaleEsistente == null)
                {
                    return NotFound();
                }

                // Aggiorna i campi modificabili
                animaleEsistente.Nome = animale.Nome;
                animaleEsistente.TipologiaAnimale = animale.TipologiaAnimale;
                animaleEsistente.ColoreManto = animale.ColoreManto;
                animaleEsistente.DataNascita = animale.DataNascita;
                animaleEsistente.Randagio = animale.Randagio;

                // Verifica se il campo NumeroChip è stato lasciato vuoto
                if (string.IsNullOrEmpty(animale.NumeroChip))
                {
                    animaleEsistente.NumeroChip = null; // Imposta NumeroChip a null
                    animaleEsistente.PossiedeChip = false; // Imposta PossiedeChip a false
                }
                else
                {
                    animaleEsistente.NumeroChip = animale.NumeroChip;
                    animaleEsistente.PossiedeChip = true; // Imposta PossiedeChip a true
                }

                // Gestione del caricamento dell'immagine
                _logger.LogInformation("Gestione del caricamento dell'immagine per l'animale con ID {Id}.", animaleEsistente.Id);

                bool shouldDeletePhoto = EliminaFoto == "on"; // Controlla se EliminaFoto è selezionato
                if (shouldDeletePhoto && !string.IsNullOrEmpty(animaleEsistente.Foto))
                {
                    // Elimina il file fisico dal percorso
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", animaleEsistente.Foto);
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                        _logger.LogInformation("Foto eliminata con successo dal percorso {FilePath}.", filePath);
                    }

                    // Rimuove il riferimento alla foto dal database
                    animaleEsistente.Foto = null;
                }

                if (img != null && img.Length > 0)
                {
                    _logger.LogInformation("File caricato con nome: {FileName} e dimensione: {Size} bytes.", img.FileName, img.Length);

                    // Definisci il percorso di salvataggio
                    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "foto");
                    Directory.CreateDirectory(uploadsFolder); // Assicurati che la cartella esista
                    var uniqueFileName = $"fotoAnimale{animaleEsistente.Id}{Path.GetExtension(img.FileName)}";
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    // Salva il file nel percorso specificato
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await img.CopyToAsync(fileStream);
                    }

                    // Aggiorna il percorso dell'immagine nel modello
                    animaleEsistente.Foto = Path.Combine("foto", uniqueFileName);
                }
                else
                {
                    _logger.LogWarning("Nessun file caricato o file caricato vuoto.");
                }

                // Salva le modifiche
                await _animaleService.UpdateAsync(animaleEsistente);

                _logger.LogInformation("Animale con ID {Id} aggiornato con successo.", animale.Id);

                return RedirectToAction(nameof(Index));
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
        }
        else
        {
            // Logga gli errori nel ModelState
            foreach (var state in ModelState)
            {
                foreach (var error in state.Value.Errors)
                {
                    _logger.LogError("Errore nel campo {Field}: {ErrorMessage}", state.Key, error.ErrorMessage);
                }
            }
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
    [AllowAnonymous]
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
    public async Task<IActionResult> Create(
        [Bind("Id,Nome,TipologiaAnimale,ColoreManto,DataNascita,PossiedeChip,NumeroChip,Randagio")] Animale animale,
        string ProprietarioNome,
        string ProprietarioCognome,
        string ProprietarioTelefono,
        string ProprietarioIndirizzo,
        string ProprietarioCitta,
        string ProprietarioCodiceFiscale,
        IFormFile? img,
        int? ProprietarioId)
    {
        // Verifica se l'animale è randagio
        if (animale.Randagio)
        {
            // Rimuovi i campi del nuovo proprietario dalla validazione
            ModelState.Remove("ProprietarioNome");
            ModelState.Remove("ProprietarioCognome");
            ModelState.Remove("ProprietarioTelefono");
            ModelState.Remove("ProprietarioIndirizzo");
            ModelState.Remove("ProprietarioCitta");
            ModelState.Remove("ProprietarioCodiceFiscale");

            // Imposta ProprietarioId a null, in modo che non venga associato un proprietario
            ProprietarioId = null;
        }

        if (ModelState.IsValid)
        {
            // Gestione della creazione o associazione del proprietario
            if (!animale.Randagio)
            {
                if (!ProprietarioId.HasValue)
                {
                    // Creazione di un nuovo proprietario
                    var nuovoProprietario = new Proprietario
                    {
                        Nome = ProprietarioNome,
                        Cognome = ProprietarioCognome,
                        Telefono = ProprietarioTelefono,
                        Indirizzo = ProprietarioIndirizzo,
                        Citta = ProprietarioCitta,
                        CodiceFiscale = ProprietarioCodiceFiscale
                    };

                    // Salvataggio del nuovo proprietario
                    nuovoProprietario = await _proprietarioService.CreateAsync(nuovoProprietario);
                    animale.ProprietarioId = nuovoProprietario.Id;
                }
                else
                {
                    // Associa l'animale a un proprietario esistente
                    animale.ProprietarioId = ProprietarioId.Value;
                }
            }

            // Salva l'animale nel database
            await _animaleService.CreateAsync(animale);

            // Gestione del caricamento dell'immagine
            if (img != null && img.Length > 0)
            {
                try
                {
                    // Salva l'immagine associata all'animale
                    _animaleService.SaveImg(animale.Id, img);
                    animale.Foto = $"foto/fotoAnimale{animale.Id}.jpg";
                    await _animaleService.UpdateAsync(animale);
                }
                catch (Exception ex)
                {
                    _logger.LogError("Errore durante il salvataggio dell'immagine: {Message}", ex.Message);
                    ModelState.AddModelError(string.Empty, "Errore durante il salvataggio dell'immagine.");
                    return View(animale);
                }
            }

            // Reindirizza all'indice degli animali
            return RedirectToAction("Index", "Animali");
        }

        // Log degli errori del ModelState
        _logger.LogError("ModelState non valido, ritorno alla vista con gli errori.");
        foreach (var state in ModelState)
        {
            var key = state.Key;
            var errors = state.Value.Errors;

            foreach (var error in errors)
            {
                _logger.LogError("Errore nel campo '{Field}': {ErrorMessage}", key, error.ErrorMessage);
            }
        }

        // Ritorna alla vista se ci sono errori di validazione
        return View(animale);
    }


}
