using ClinicaVeterinaria.Models;
using ClinicaVeterinaria.Service.Intertface;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace ClinicaVeterinaria.Service
{
    public class UtenteService : IUtenteService
    {
        private readonly VeterinaryClinicContext _context;

        public UtenteService(VeterinaryClinicContext context)
        {
            _context = context;
        }

        public async Task<Utente> CreateUtenteAsync(Utente utente)
        {
            // Imposta il ruolo predefinito se non specificato
            if (string.IsNullOrEmpty(utente.Ruolo))
            {
                utente.Ruolo = "User";
            }

            // Verifica che la password sia stata impostata e genera gli hash/salt
            if (!string.IsNullOrEmpty(utente.Password))
            {
                utente.SetPassword(utente.Password);
            }

            _context.Utenti.Add(utente);
            await _context.SaveChangesAsync();
            return utente;
        }

        public async Task<Utente> GetUtenteByIdAsync(int utenteId)
        {
            return await _context.Utenti.FindAsync(utenteId);
        }

        public async Task<IEnumerable<Utente>> GetAllUtentiAsync()
        {
            return await _context.Utenti.ToListAsync();
        }

        public async Task<Utente> UpdateUtenteAsync(Utente utente)
        {
            _context.Entry(utente).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return utente;
        }

        public async Task DeleteUtenteAsync(int utenteId)
        {
            var utente = await _context.Utenti.FindAsync(utenteId);
            if (utente != null)
            {
                _context.Utenti.Remove(utente);
                await _context.SaveChangesAsync();
            }
        }

        public Utente Login(string username, string password)
        {
            var utente = _context.Utenti.SingleOrDefault(u => u.Username == username);
            if (utente == null || !utente.VerifyPassword(password))
            {
                return null; // Utente non trovato o password errata
            }
            return utente; // Login avvenuto con successo
        }
    }
}
