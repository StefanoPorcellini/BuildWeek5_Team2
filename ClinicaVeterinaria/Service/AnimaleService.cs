using ClinicaVeterinaria.Models;
using ClinicaVeterinaria.Service.Intertface;
using Microsoft.EntityFrameworkCore;

namespace ClinicaVeterinaria.Service
{
    public class AnimaleService : IAnimaleService
    {
        private readonly VeterinaryClinicContext _context;
        private readonly ILogger<AnimaleService> _logger;


        public AnimaleService(VeterinaryClinicContext context, ILogger<AnimaleService> logger)
        {
            _context = context;
            _logger = logger;
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
        public void SaveImg(int animaleId, IFormFile img)
        {
            if (img == null || img.Length == 0)
            {
                _logger.LogError("L'immagine è null o vuota.");
                return;
            }

            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/foto", $"fotoAnimale{animaleId}.jpg");

            try
            {
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    img.CopyTo(stream);
                }
                _logger.LogInformation("Immagine salvata con successo in {Path}", path);
            }
            catch (Exception ex)
            {
                _logger.LogError("Errore durante il salvataggio dell'immagine: {Message}", ex.Message);
                throw;
            }
        }

    }
}
