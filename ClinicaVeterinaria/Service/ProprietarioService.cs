using ClinicaVeterinaria.Models;
using ClinicaVeterinaria.Service.Intertface;
using Microsoft.EntityFrameworkCore;

namespace ClinicaVeterinaria.Service
{
    public class ProprietarioService : IProprietarioService
    {
        private readonly VeterinaryClinicContext _context;

        public ProprietarioService(VeterinaryClinicContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Proprietario>> GetAllAsync()
        {
            return await _context.Proprietari.ToListAsync();
        }

        public async Task<Proprietario> GetByIdAsync(int id)
        {
            return await _context.Proprietari
                .Include(p => p.Animali)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Proprietario> CreateAsync(Proprietario proprietario)
        {
            _context.Proprietari.Add(proprietario);
            await _context.SaveChangesAsync();
            return proprietario;
        }

        public async Task<Proprietario> UpdateAsync(Proprietario proprietario)
        {
            _context.Proprietari.Update(proprietario);
            await _context.SaveChangesAsync();
            return proprietario;
        }

        public async Task DeleteAsync(int id)
        {
            var proprietario = await _context.Proprietari.FindAsync(id);
            if (proprietario != null)
            {
                _context.Proprietari.Remove(proprietario);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Proprietario>> SearchProprietariAsync(string term)
        {
            if (string.IsNullOrWhiteSpace(term))
            {
                return new List<Proprietario>();
            }

            return await _context.Proprietari
                .Where(p => p.Nome.Contains(term) || p.Cognome.Contains(term) || p.CodiceFiscale.Contains(term))
                .ToListAsync();
        }
    }
}
