using ClinicaVeterinaria.Models;
using ClinicaVeterinaria.Service.Intertface;
using Microsoft.EntityFrameworkCore;

namespace ClinicaVeterinaria.Service
{
    public class AnimaleService : IAnimaleService
    {
        private readonly VeterinaryClinicContext _context;

        public AnimaleService(VeterinaryClinicContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Animale>> GetAllAsync()
        {
            return await _context.Animali.ToListAsync();
        }

        public async Task<Animale> GetByIdAsync(int id)
        {
            return await _context.Animali
                                 .Include(a => a.Proprietario) // Include il proprietario associato
                                 .FirstOrDefaultAsync(m => m.Id == id);
        }


        public async Task<Animale> CreateAsync(Animale animale)
        {
            _context.Animali.Add(animale);
            await _context.SaveChangesAsync();
            return animale;
        }

        public async Task<Animale> UpdateAsync(Animale animale)
        {
            _context.Animali.Update(animale);
            await _context.SaveChangesAsync();
            return animale;
        }

        public async Task DeleteAsync(int id)
        {

            DeleteImg(id);
            var animale = await _context.Animali.FindAsync(id);
            if (animale != null)
            {
                _context.Animali.Remove(animale);
                await _context.SaveChangesAsync();
            }
        }

        private void DeleteImg(int idAnimale) 
        {
            var imgPath = Path.Combine("wwwroot", "foto", $"fotoAnimale{idAnimale}.jpg");
            if (File.Exists(imgPath))
            {
                File.Delete(imgPath);
            }
        }

        public bool AnimaleExists(int id)
        {
            return _context.Animali.Any(e => e.Id == id);
        }
        public async Task<Proprietario> GetProprietarioByIdAsync(int proprietarioId)
        {
            return await _context.Proprietari.FindAsync(proprietarioId);
        }

        public async Task<Animale> SearchByChipNumberAsync(string chipNumber)
        {
            return await _context.Animali
                .FirstOrDefaultAsync(a => a.NumeroChip == chipNumber);
        }

        public void SaveImg(int idAnimale, IFormFile img)
        {
            var imgPath = Path.Combine("wwwroot", "foto", $"fotoAnimale{idAnimale}.jpg");

            using (var stream = new FileStream(imgPath, FileMode.Create))
            {
                img.CopyTo(stream);
            }
        }
    }
}
