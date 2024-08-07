using ClinicaVeterinaria.Interface;
using ClinicaVeterinaria.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

[Authorize(Roles = "Veterinario,Admin")] 
public class ContabilizzazioneController : Controller
{
    private readonly IRicoveroService _ricoveroService;

    public ContabilizzazioneController(IRicoveroService ricoveroService)
    {
        _ricoveroService = ricoveroService;
    }

    public async Task<IActionResult> Index()
    {
        var ricoveriAttivi = await _ricoveroService.GetRicoveriAttiviAsync();
        return View(ricoveriAttivi);
    }

    // Metodo asincrono per ottenere i ricoveri attivi
    [HttpGet]
    public async Task<IActionResult> GetRicoveriAttiviReport()
    {
        var ricoveriAttivi = await _ricoveroService.GetRicoveriAttiviAsync();
        return PartialView("_RicoveriAttiviPartial", ricoveriAttivi);
    }

}
