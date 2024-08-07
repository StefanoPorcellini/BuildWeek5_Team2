using ClinicaVeterinaria.Models;
using ClinicaVeterinaria.Service.Interface;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ClinicaVeterinaria.Service
{
    public class VenditaService : IVenditaService
    {
        private readonly VeterinaryClinicContext _context;

        public VenditaService(VeterinaryClinicContext context)
        {
            _context = context;
        }

        public async Task<List<Vendita>> GetAllAsync()
        {
            return await _context.Vendite.Include(v => v.Prodotto).ToListAsync();
        }

        public async Task<Vendita> GetByIdAsync(int id)
        {
            return await _context.Vendite.Include(v => v.Prodotto).FirstOrDefaultAsync(v => v.Id == id);
        }

        public async Task CreateAsync(Vendita vendita)
        {
            _context.Vendite.Add(vendita);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Vendita vendita)
        {
            _context.Vendite.Update(vendita);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var vendita = await _context.Vendite.FindAsync(id);
            if (vendita != null)
            {
                _context.Vendite.Remove(vendita);
                await _context.SaveChangesAsync();
            }
        }
        public async Task<string> GetCodiceFiscaleByNomeOrCodiceAsync(string nome, string codiceFiscale)
        {
            // Cerca il codice fiscale nella tabella Clienti
            var cliente = await _context.Clienti
                .FirstOrDefaultAsync(c => c.CodiceFiscale == codiceFiscale || c.Nome == nome);

            if (cliente != null)
            {
                return cliente.CodiceFiscale;
            }

            // Cerca il codice fiscale nella tabella Proprietari
            var proprietario = await _context.Proprietari
                .FirstOrDefaultAsync(p => p.CodiceFiscale == codiceFiscale || p.Nome == nome);

            return proprietario?.CodiceFiscale;
        }
        public async Task<bool> VenditaExistsAsync(int id)
        {
            return await _context.Vendite.AnyAsync(v => v.Id == id);
        }
    }
}
