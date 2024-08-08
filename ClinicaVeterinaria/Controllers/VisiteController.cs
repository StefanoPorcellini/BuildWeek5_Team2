using ClinicaVeterinaria.Models;
using ClinicaVeterinaria.Service.Intertface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;


[Authorize(Roles = "Veterinario,Admin")]
public class VisiteController : Controller
{
    private readonly IVisitaService _visitaService;
    private readonly VeterinaryClinicContext _context;

    public VisiteController(IVisitaService visitaService, VeterinaryClinicContext context)
    {
        _visitaService = visitaService;
        _context = context;
    }

    // GET: Visite
    public async Task<IActionResult> Index()
    {
        var visite = await _context.Visite.ToListAsync();
        var animali = await _context.Animali
            .Select(a => new
            {
                a.Id,
                Nome = a.Nome ?? "Senza Nome"
            })
            .ToListAsync();

        var visiteWithAnimaleName = visite.Select(v => new
        {
            v.Id,
            v.DataVisita,
            AnimaleNome = animali.FirstOrDefault(a => a.Id == v.AnimaleId)?.Nome ?? "Senza Nome",
            v.Anamnesi,
            v.Cura,
            v.Prezzo
        }).ToList();

        return View(visiteWithAnimaleName);
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

        var animale = await _context.Animali
            .Where(a => a.Id == visita.AnimaleId)
            .Select(a => new { a.Id, Nome = a.Nome ?? "Senza Nome" })
            .FirstOrDefaultAsync();

        ViewBag.AnimaleNome = animale?.Nome ?? "Senza Nome";

        return View(visita);
    }

    // GET: Visite/Create
    public async Task<IActionResult> Create(int animaleId)
    {
        var animali = await _context.Animali
            .Select(a => new
            {
                a.Id,
                Nome = a.Nome ?? "Senza Nome"
            })
            .ToListAsync();

        ViewBag.AnimaleId = new SelectList(animali, "Id", "Nome", animaleId);

        var visita = new Visita
        {
            AnimaleId = animaleId,
            DataVisita = DateTime.Now // Imposta la data corrente
        };

        return View(visita);
    }

    // POST: Visite/Create
    [HttpPost]
    public async Task<IActionResult> Create([Bind("Id,DataVisita,AnimaleId,Anamnesi,Cura,Prezzo")] Visita visita)
    {
        if (ModelState.IsValid)
        {
            visita.DataVisita = DateTime.Now; // Imposta la data corrente
            visita.DataVisita = visita.DataVisita.AddTicks(-(visita.DataVisita.Ticks % TimeSpan.TicksPerSecond)); // Rimuovi i millisecondi

            await _visitaService.CreateAsync(visita);
            return RedirectToAction(nameof(Index), new { animaleId = visita.AnimaleId });
        }

        var animali = await _context.Animali
            .Select(a => new
            {
                a.Id,
                Nome = a.Nome ?? "Senza Nome"
            })
            .ToListAsync();

        ViewBag.AnimaleId = new SelectList(animali, "Id", "Nome", visita.AnimaleId);
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

        var animale = await _context.Animali
            .Where(a => a.Id == visita.AnimaleId)
            .Select(a => new { a.Id, Nome = a.Nome ?? "Senza Nome" })
            .FirstOrDefaultAsync();

        ViewBag.AnimaleNome = animale?.Nome ?? "Senza Nome";

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


