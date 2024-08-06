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
            var visite = await _context.Visite
                .Where(v => v.AnimaleId == animaleId)
                .OrderByDescending(v => v.DataVisita)
                .ToListAsync();

            var animali = await _context.Animali
                .Where(a => visite.Select(v => v.AnimaleId).Contains(a.Id))
                .ToDictionaryAsync(a => a.Id, a => a.Nome ?? "Senza Nome");

            foreach (var visita in visite)
            {
                visita.Animale = new Animale { Id = visita.AnimaleId, Nome = animali[visita.AnimaleId] };
            }

            return visite;
        }

        public async Task<Visita> GetByIdAsync(int id)
        {
            var visita = await _context.Visite
                .FirstOrDefaultAsync(v => v.Id == id);

            if (visita != null)
            {
                var animale = await _context.Animali
                    .Where(a => a.Id == visita.AnimaleId)
                    .Select(a => new { a.Id, Nome = a.Nome ?? "Senza Nome" })
                    .FirstOrDefaultAsync();

                if (animale != null)
                {
                    visita.Animale = new Animale { Id = animale.Id, Nome = animale.Nome };
                }
            }

            return visita;
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

        public async Task DeleteAsync(Visita visita)
        {
                _context.Visite.Remove(visita);
                await _context.SaveChangesAsync();
        }

        public bool VisitaExists(int id)
        {
            return _context.Visite.Any(e => e.Id == id);
        }
    }
}

