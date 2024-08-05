using ClinicaVeterinaria.Models;
using ClinicaVeterinaria.Service.Intertface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class ProprietariController : Controller
{
    private readonly IProprietarioService _proprietarioService;

    public ProprietariController(IProprietarioService proprietarioService)
    {
        _proprietarioService = proprietarioService;
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

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Nome,Cognome,Telefono,Indirizzo,Citta,CodiceFiscale")] Proprietario proprietario)
    {
        if (id != proprietario.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                await _proprietarioService.UpdateAsync(proprietario);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (await _proprietarioService.GetByIdAsync(proprietario.Id) == null)
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
        return View(proprietario);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _proprietarioService.DeleteAsync(id);
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Search(string term)
    {
        var proprietari = await _proprietarioService.SearchProprietariAsync(term);

        var result = proprietari.Select(p => new
        {
            p.Id,
            NomeCompleto = $"{p.Nome} {p.Cognome} ({p.CodiceFiscale})"
        }).ToList();

        return Json(result);
    }
}
