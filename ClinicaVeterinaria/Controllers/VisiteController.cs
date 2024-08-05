using ClinicaVeterinaria.Models;
using ClinicaVeterinaria.Service.Intertface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

public class VisiteController : Controller
{
    private readonly IVisitaService _visitaService;

    public VisiteController(IVisitaService visitaService)
    {
        _visitaService = visitaService;
    }

    // GET: Visite
    public async Task<IActionResult> Index(int animaleId)
    {
        var visite = await _visitaService.GetAllByAnimaleIdAsync(animaleId);
        return View(visite);
    }

    // GET: Visite/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var visita = await _visitaService.GetByIdAsync(id.Value);
        if (visita == null)
        {
            return NotFound();
        }

        return View(visita);
    }

    // GET: Visite/Create
    public IActionResult Create(int animaleId)
    {
        ViewBag.AnimaleId = animaleId;
        return View();
    }

    // POST: Visite/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,DataVisita,AnimaleId,Anamnesi,Cura,Prezzo")] Visita visita)
    {
        if (ModelState.IsValid)
        {
            await _visitaService.CreateAsync(visita);
            return RedirectToAction(nameof(Index), new { animaleId = visita.AnimaleId });
        }
        return View(visita);
    }

    // GET: Visite/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var visita = await _visitaService.GetByIdAsync(id.Value);
        if (visita == null)
        {
            return NotFound();
        }
        return View(visita);
    }

    // POST: Visite/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,DataVisita,AnimaleId,Anamnesi,Cura,Prezzo")] Visita visita)
    {
        if (id != visita.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                await _visitaService.UpdateAsync(visita);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_visitaService.VisitaExists(visita.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index), new { animaleId = visita.AnimaleId });
        }
        return View(visita);
    }

}


