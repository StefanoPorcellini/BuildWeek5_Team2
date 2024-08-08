using ClinicaVeterinaria.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

public class ClientiController : Controller
{
    private readonly IClienteService _clienteService;

    public ClientiController(IClienteService clienteService)
    {
        _clienteService = clienteService;
    }

    // GET: Clienti
    public async Task<IActionResult> Index()
    {
        var clienti = await _clienteService.GetAllAsync();
        return View(clienti);
    }

    // GET: Clienti/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var cliente = await _clienteService.GetByIdAsync(id.Value);

        if (cliente == null)
        {
            return NotFound();
        }

        return View(cliente);
    }

    // GET: Clienti/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Clienti/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Nome,Cognome,Telefono,Indirizzo,Citta,CodiceFiscale")] Cliente cliente)
    {
        if (ModelState.IsValid)
        {
            await _clienteService.CreateAsync(cliente);
            return RedirectToAction(nameof(Index));
        }
        return View(cliente);
    }

    // GET: Clienti/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var cliente = await _clienteService.GetByIdAsync(id.Value);
        if (cliente == null)
        {
            return NotFound();
        }
        return View(cliente);
    }

    // POST: Clienti/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Nome,Cognome,Telefono,Indirizzo,Citta,CodiceFiscale")] Cliente cliente)
    {
        if (id != cliente.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                await _clienteService.UpdateAsync(cliente);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _clienteService.ClienteExistsAsync(cliente.Id))
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
        return View(cliente);
    }

    // GET: Clienti/Search
    [HttpGet]
    public async Task<IActionResult> Search(string term)
    {
        var risultati = await _clienteService.SearchAsync(term);
        return Json(risultati);
    }
}
