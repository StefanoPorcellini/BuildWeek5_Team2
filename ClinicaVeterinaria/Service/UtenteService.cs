using ClinicaVeterinaria.Models;
using ClinicaVeterinaria.Service.Intertface;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace ClinicaVeterinaria.Service
using ClinicaVeterinaria.Intertface;
using ClinicaVeterinaria.Service;
using Microsoft.EntityFrameworkCore;

namespace ClinicaVeterinaria.Services
{
    public class UtenteService : IUtenteService
    {
        private readonly VeterinaryClinicContext _context;
        private readonly ILogger<UtenteService> _logger;

        public UtenteService(VeterinaryClinicContext context, ILogger<UtenteService> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<Utente> CreateUtenteAsync(Utente utente)
        {
            try
            {
                _logger.LogInformation("Tentativo di creazione di un nuovo utente: {Username}", utente.Username);
                _context.Utenti.Add(utente);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Utente {Username} creato con successo.", utente.Username);
                return utente;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Errore durante la creazione dell'utente {Username}", utente.Username);
                throw;
            }
        }


        public async Task<Utente> GetUtenteByIdAsync(int utenteId)
        {
            try
            {
                return await _context.Utenti.FindAsync(utenteId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Errore durante la ricerca dell'utente con ID {UtenteId}", utenteId);
                throw;
            }
        }

        public async Task<IEnumerable<Utente>> GetAllUtentiAsync()
        {
            try
            {
                return await _context.Utenti.ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Errore durante il recupero di tutti gli utenti.");
                throw;
            }
        }

        public async Task<Utente> UpdateUtenteAsync(Utente utente)
        {
            try
            {
                _context.Entry(utente).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                _logger.LogInformation("Utente {Username} aggiornato con successo.", utente.Username);
                return utente;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Errore durante l'aggiornamento dell'utente {Username}", utente.Username);
                throw;
            }
        }

        public async Task DeleteUtenteAsync(int utenteId)
        {
            try
            {
                var utente = await _context.Utenti.FindAsync(utenteId);
                if (utente != null)
                {
                    _context.Utenti.Remove(utente);
                    await _context.SaveChangesAsync();
                    _logger.LogInformation("Utente con ID {UtenteId} eliminato con successo.", utenteId);
                }
                else
                {
                    _logger.LogWarning("Utente con ID {UtenteId} non trovato per l'eliminazione.", utenteId);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Errore durante l'eliminazione dell'utente con ID {UtenteId}", utenteId);
                throw;
            }
        }

        public async Task<Utente> LoginAsync(string username, string password)
        {
            try
            {
                var user = await _context.Utenti.SingleOrDefaultAsync(u => u.Username == username);
                if (user == null)
                {
                    _logger.LogWarning("Tentativo di login fallito per username: {Username}. Utente non trovato.", username);
                    return null;
                }

                bool isValid = PasswordService.VerifyPassword(password, user.PasswordSalt, user.PasswordHash);
                if (isValid)
                {
                    _logger.LogInformation("Login riuscito per username: {Username}.", username);
                    return user;
                }
                else
                {
                    _logger.LogWarning("Tentativo di login fallito per username: {Username}. Password non valida.", username);
                    return null;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Errore durante il login per username: {Username}", username);
                throw;
            }
        }
    }
}
