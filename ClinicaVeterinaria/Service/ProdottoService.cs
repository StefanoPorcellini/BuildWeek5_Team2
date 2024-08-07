using ClinicaVeterinaria.Interface;
using ClinicaVeterinaria.Models;
using ClinicaVeterinaria.Models.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ClinicaVeterinaria.Services
{
    public class ProdottoService : IProdottoService
    {
        private readonly VeterinaryClinicContext _context;
        private List<Prodotto> _prodottiMemoria; // Lista per memorizzare i prodotti

        // Costruttore che inietta il contesto del database
        public ProdottoService(VeterinaryClinicContext context)
        {
            _context = context;
        }

        // Metodo per ottenere tutte le case farmaceutiche dal database
        public async Task<IEnumerable<CasaFarmaceutica>> GetCaseFarmaceuticheAsync()
        {
            return await _context.CaseFarmaceutiche.ToListAsync();
        }

        // Metodo per aggiungere un nuovo prodotto nel database
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
        }

        // Metodo per aggiungere una nuova casa farmaceutica nel database
        public async Task<int> AddCasaFarmaceuticaAsync(CasaFarmaceutica casaFarmaceutica)
        {
            _context.CaseFarmaceutiche.Add(casaFarmaceutica);
            await _context.SaveChangesAsync();
            return casaFarmaceutica.Id;
        }

        // Metodo per ottenere tutti i prodotti dal database
        public async Task<IEnumerable<Prodotto>> GetAllProdottiAsync()
        {
            return await _context.Prodotti.Include(p => p.CasaFarmaceutica).ToListAsync();
        }

        // Metodo per ottenere un prodotto specifico dal database
        public async Task<Prodotto> GetProdottoByIdAsync(int id)
        {
            return await _context.Prodotti.Include(p => p.CasaFarmaceutica)
                                          .FirstOrDefaultAsync(p => p.Id == id);
        }

        // Metodo per aggiornare un prodotto esistente nel database
        public async Task UpdateProdottoAsync(Prodotto prodotto)
        {
            _context.Prodotti.Update(prodotto);
            await _context.SaveChangesAsync();
        }

        // Metodo per cancellare un prodotto dal database
        public async Task DeleteProdottoAsync(int id)
        {
            var prodotto = await _context.Prodotti.FindAsync(id);
            if (prodotto != null)
            {
                _context.Prodotti.Remove(prodotto);
                await _context.SaveChangesAsync();
            }
        }

        // Metodo per ottenere una casa farmaceutica specifica dal database
        public async Task<CasaFarmaceutica> GetCasaFarmaceuticaByIdAsync(int id)
        {
            return await _context.CaseFarmaceutiche.FindAsync(id);
        }

        // Metodo per aggiornare una casa farmaceutica esistente nel database
        public async Task UpdateCasaFarmaceuticaAsync(CasaFarmaceutica casaFarmaceutica)
        {
            _context.CaseFarmaceutiche.Update(casaFarmaceutica);
            await _context.SaveChangesAsync();
        }

        // Metodo per cancellare una casa farmaceutica dal database
        public async Task DeleteCasaFarmaceuticaAsync(int id)
        {
            var casaFarmaceutica = await _context.CaseFarmaceutiche.FindAsync(id);
            if (casaFarmaceutica != null)
            {
                _context.CaseFarmaceutiche.Remove(casaFarmaceutica);
                await _context.SaveChangesAsync();
            }
        }

        // Implementazione del metodo GetProdottiMemoria
        public List<Prodotto> GetProdottiMemoria()
        {
            if (_prodottiMemoria == null)
            {
                _prodottiMemoria = _context.Prodotti.ToList(); // Carica i prodotti dal database
            }
            return _prodottiMemoria;
        }
    }
}
