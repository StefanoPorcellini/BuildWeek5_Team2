using ClinicaVeterinaria.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

public class ClientiController : Controller
{
    private readonly VeterinaryClinicContext _context;

    public ClientiController(VeterinaryClinicContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> Search(string term)
    {
        var clienti = await _context.Clienti
            .Where(c => c.Nome.Contains(term) || c.CodiceFiscale.Contains(term))
            .Select(c => new
            {
                Nome = c.Nome,
                Cognome = c.Cognome,
                CodiceFiscale = c.CodiceFiscale
            })
            .ToListAsync();

        var proprietari = await _context.Proprietari
            .Where(p => p.Nome.Contains(term) || p.CodiceFiscale.Contains(term))
            .Select(p => new
            {
                Nome = p.Nome,
                Cognome = p.Cognome,
                CodiceFiscale = p.CodiceFiscale
            })
            .ToListAsync();

        var risultati = clienti.Concat(proprietari).ToList();

        return Json(risultati);
    }

}
