using ClinicaVeterinaria.Interface;
using ClinicaVeterinaria.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ClinicaVeterinaria.Service
{
    public class RicoveroService : IRicoveroService
    {
        private readonly VeterinaryClinicContext _context;
        private readonly ILogger<RicoveroService> _logger;

        public RicoveroService(VeterinaryClinicContext context,ILogger<RicoveroService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<List<Ricovero>> GetAllAsync()
        {
            return await _context.Ricoveri.Include(r => r.Animale).ToListAsync();
        }

        public async Task<Ricovero> GetByIdAsync(int id)
        {
            return await _context.Ricoveri.Include(r => r.Animale).FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<Ricovero> GetRicoveroWithAnimaleAsync(int id)
        {
            return await _context.Ricoveri
                .Include(r => r.Animale)
                .FirstOrDefaultAsync(r => r.Id == id);
        }
        public async Task<decimal> CalcolaCostoTotaleAsync(int ricoveroId, DateTime? dataFine = null)
        {
            _logger.LogInformation("Calcolo del costo totale per il ricovero con ID: {RicoveroId}", ricoveroId);

            var ricovero = await _context.Ricoveri.FindAsync(ricoveroId);
            if (ricovero == null)
            {
                _logger.LogWarning("Ricovero con ID: {RicoveroId} non trovato.", ricoveroId);
                throw new ArgumentException("Ricovero non trovato.");
            }

            var dataInizio = ricovero.DataInizio;
            var costoGiornaliero = ricovero.CostoGiornaliero;
            var dataEffettivaFine = dataFine ?? DateTime.Now;

            _logger.LogInformation("Data Inizio: {DataInizio}, Data Fine: {DataFine}, Costo Giornaliero: {CostoGiornaliero}", dataInizio, dataEffettivaFine, costoGiornaliero);

            var giorniDiRicovero = (dataEffettivaFine - dataInizio).Days;
            if (giorniDiRicovero <= 0)
            {
                giorniDiRicovero = 1;
            }

            var costoTotale = giorniDiRicovero * costoGiornaliero;

            _logger.LogInformation("Costo totale calcolato per il ricovero con ID: {RicoveroId} è: {CostoTotale}", ricoveroId, costoTotale);

            // Aggiorna il PrezzoTotale e lo stato Dimesso
            ricovero.PrezzoTotale = costoTotale;
            ricovero.Dimesso = costoTotale > 0;

            await _context.SaveChangesAsync();

            return costoTotale;
        }

        public async Task<List<Ricovero>> GetRicoveriAttiviAsync() 
        {
            return await _context.Ricoveri
                .Include(r => r.Animale)
                .Where(r =>!r.Dimesso) 
                .ToListAsync();
        }
        public async Task CreateAsync(Ricovero ricovero)
        {
            ricovero.DataInizio = DateTime.Now;
            _context.Ricoveri.Add(ricovero);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Ricovero ricovero)
        {
            _context.Ricoveri.Update(ricovero);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var ricovero = await GetByIdAsync(id);
            if (ricovero != null)
            {
                _context.Ricoveri.Remove(ricovero);
                await _context.SaveChangesAsync();
            }
        }
    }
}
