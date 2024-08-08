using ClinicaVeterinaria.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class ClienteService : IClienteService
{
    private readonly VeterinaryClinicContext _context;

    public ClienteService(VeterinaryClinicContext context)
    {
        _context = context;
    }

    public async Task<List<Cliente>> GetAllAsync()
    {
        return await _context.Clienti.ToListAsync();
    }

    public async Task<Cliente> GetByIdAsync(int id)
    {
        return await _context.Clienti.FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task CreateAsync(Cliente cliente)
    {
        _context.Clienti.Add(cliente);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Cliente cliente)
    {
        _context.Clienti.Update(cliente);
        await _context.SaveChangesAsync();
    }

    public async Task<List<object>> SearchAsync(string term)
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

        return clienti.Concat(proprietari).Cast<object>().ToList();
    }

    public async Task<bool> ClienteExistsAsync(int id)
    {
        return await _context.Clienti.AnyAsync(e => e.Id == id);
    }
}
