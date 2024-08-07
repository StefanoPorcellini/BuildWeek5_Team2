using ClinicaVeterinaria.Interface;
using ClinicaVeterinaria.Models;
using ClinicaVeterinaria.Models.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace ClinicaVeterinaria.Services
{
    public class ProdottoService : IProdottoService
    {
        private readonly VeterinaryClinicContext _context;
        private readonly ILogger<ProdottoService> _logger;

        public ProdottoService(VeterinaryClinicContext context, ILogger<ProdottoService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<CasaFarmaceutica>> GetCaseFarmaceuticheAsync()
        {
            _logger.LogInformation("Caricamento delle case farmaceutiche dal database.");
            return await _context.CaseFarmaceutiche.ToListAsync();
        }
        public async Task AddProdottoAsync(ProdottoViewModel prodottoViewModel)
        {
            var prodotto = new Prodotto
            {
                Nome = prodottoViewModel.Nome,
                Prezzo = prodottoViewModel.Prezzo,
                NumeroArmadietto = prodottoViewModel.NumeroArmadietto,
                NumeroCassetto = prodottoViewModel.NumeroCassetto,
                Tipologia = prodottoViewModel.Tipologia,
                CasaFarmaceuticaId = prodottoViewModel.SelectedCasaFarmaceuticaId.Value
            };

            _context.Prodotti.Add(prodotto);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Prodotto aggiunto con successo.");
        }

        public async Task<int> AddCasaFarmaceuticaAsync(CasaFarmaceutica casaFarmaceutica)
        {
            _context.CaseFarmaceutiche.Add(casaFarmaceutica);
            await _context.SaveChangesAsync();
            _logger.LogInformation($"Casa farmaceutica '{casaFarmaceutica.Nome}' aggiunta con successo.");
            return casaFarmaceutica.Id;
        }

        public async Task<IEnumerable<Prodotto>> GetAllProdottiAsync()
        {
            _logger.LogInformation("Caricamento di tutti i prodotti dal database.");
            return await _context.Prodotti.Include(p => p.CasaFarmaceutica).ToListAsync();
        }

        public async Task<Prodotto> GetProdottoByIdAsync(int id)
        {
            _logger.LogInformation($"Ricerca del prodotto con ID {id}.");
            return await _context.Prodotti.Include(p => p.CasaFarmaceutica)
                                          .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task UpdateProdottoAsync(Prodotto prodotto)
        {
            _logger.LogInformation($"Aggiornamento del prodotto con ID {prodotto.Id}.");
            _context.Prodotti.Update(prodotto);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteProdottoAsync(int id)
        {
            _logger.LogInformation($"Cancellazione del prodotto con ID {id}.");
            var prodotto = await _context.Prodotti.FindAsync(id);
            if (prodotto != null)
            {
                _context.Prodotti.Remove(prodotto);
                await _context.SaveChangesAsync();
                _logger.LogInformation($"Prodotto con ID {id} cancellato con successo.");
            }
            else
            {
                _logger.LogWarning($"Prodotto con ID {id} non trovato.");
            }
        }

        public async Task<CasaFarmaceutica> GetCasaFarmaceuticaByIdAsync(int id)
        {
            _logger.LogInformation($"Ricerca della casa farmaceutica con ID {id}.");
            return await _context.CaseFarmaceutiche.FindAsync(id);
        }

        public async Task UpdateCasaFarmaceuticaAsync(CasaFarmaceutica casaFarmaceutica)
        {
            _logger.LogInformation($"Aggiornamento della casa farmaceutica con ID {casaFarmaceutica.Id}.");
            _context.CaseFarmaceutiche.Update(casaFarmaceutica);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteCasaFarmaceuticaAsync(int id)
        {
            _logger.LogInformation($"Cancellazione della casa farmaceutica con ID {id}.");
            var casaFarmaceutica = await _context.CaseFarmaceutiche.FindAsync(id);
            if (casaFarmaceutica != null)
            {
                _context.CaseFarmaceutiche.Remove(casaFarmaceutica);
                await _context.SaveChangesAsync();
                _logger.LogInformation($"Casa farmaceutica con ID {id} cancellata con successo.");
            }
            else
            {
                _logger.LogWarning($"Casa farmaceutica con ID {id} non trovata.");
            }
        }
    }
}
