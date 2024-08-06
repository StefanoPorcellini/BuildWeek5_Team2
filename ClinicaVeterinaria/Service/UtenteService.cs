using ClinicaVeterinaria.Models;
using ClinicaVeterinaria.Interface;
using Microsoft.EntityFrameworkCore;
using ClinicaVeterinaria.Service;

namespace ClinicaVeterinaria.Services
{
    public class UtenteService : IUtenteService
    {
        private readonly VeterinaryClinicContext _context;

        public UtenteService(VeterinaryClinicContext context)
        {
            _context = context;
        }

        // Metodo per creare un nuovo utente nel database
        public async Task<Utente> CreateUtenteAsync(Utente utente)
        {
            try
            {
                _context.Utenti.Add(utente);
                await _context.SaveChangesAsync();
                return utente;
            }
            catch (Exception ex)
            {
                // Gestione degli errori durante la creazione dell'utente
                throw;
            }
        }

        // Metodo per ottenere un utente per ID
        public async Task<Utente> GetUtenteByIdAsync(int utenteId)
        {
            try
            {
                return await _context.Utenti.FindAsync(utenteId);
            }
            catch (Exception ex)
            {
                // Gestione degli errori durante la ricerca dell'utente per ID
                throw;
            }
        }

        // Metodo per ottenere tutti gli utenti
        public async Task<IEnumerable<Utente>> GetAllUtentiAsync()
        {
            try
            {
                return await _context.Utenti.ToListAsync();
            }
            catch (Exception ex)
            {
                // Gestione degli errori durante il recupero di tutti gli utenti
                throw;
            }
        }

        // Metodo per aggiornare un utente
        public async Task<Utente> UpdateUtenteAsync(Utente utente)
        {
            try
            {
                _context.Entry(utente).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return utente;
            }
            catch (Exception ex)
            {
                // Gestione degli errori durante l'aggiornamento dell'utente
                throw;
            }
        }

        // Metodo per eliminare un utente
        public async Task DeleteUtenteAsync(int utenteId)
        {
            try
            {
                var utente = await _context.Utenti.FindAsync(utenteId);
                if (utente != null)
                {
                    _context.Utenti.Remove(utente);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    // Gestione del caso in cui l'utente non viene trovato per l'eliminazione
                }
            }
            catch (Exception ex)
            {
                // Gestione degli errori durante l'eliminazione dell'utente
                throw;
            }
        }

        // Metodo per eseguire il login di un utente
        public async Task<Utente> LoginAsync(string username, string password)
        {
            try
            {
                var user = await _context.Utenti.SingleOrDefaultAsync(u => u.Username == username);
                if (user == null)
                {
                    // Gestione del caso in cui l'utente non viene trovato
                    return null;
                }

                bool isValid = PasswordService.VerifyPassword(password, user.PasswordSalt, user.PasswordHash);
                if (isValid)
                {
                    return user;
                }
                else
                {
                    // Gestione del caso in cui la password non è valida
                    return null;
                }
            }
            catch (Exception ex)
            {
                // Gestione degli errori durante il login
                throw;
            }
        }
    }
}
