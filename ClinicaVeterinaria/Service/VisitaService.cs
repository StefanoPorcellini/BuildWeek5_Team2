using ClinicaVeterinaria.Models;
using ClinicaVeterinaria.Service.Intertface;
using Microsoft.EntityFrameworkCore;

namespace ClinicaVeterinaria.Service
{
    public class VisitaService : IVisitaService
    {
        private readonly VeterinaryClinicContext _context;

        public VisitaService(VeterinaryClinicContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Visita>> GetAllByAnimaleIdAsync(int animaleId)
        {
            return await _context.Visite
                .Where(v => v.AnimaleId == animaleId)
                .OrderByDescending(v => v.DataVisita)
                .Include(v => v.Animale)
                .ToListAsync();
        }

        public async Task<Visita> GetByIdAsync(int id)
        {
            return await _context.Visite
                .Include(v => v.Animale)
                .FirstOrDefaultAsync(v => v.Id == id);
        }

        public async Task<Visita> CreateAsync(Visita visita)
        {
            _context.Visite.Add(visita);
            await _context.SaveChangesAsync();
            return visita;
        }

        public async Task<Visita> UpdateAsync(Visita visita)
        {
            _context.Visite.Update(visita);
            await _context.SaveChangesAsync();
            return visita;
        }

        public async Task DeleteAsync(int id)
        {
            var visita = await _context.Visite.FindAsync(id);
            if (visita != null)
            {
                _context.Visite.Remove(visita);
                await _context.SaveChangesAsync();
            }
        }

        public bool VisitaExists(int id)
        {
            return _context.Visite.Any(e => e.Id == id);
        }
    }

}
