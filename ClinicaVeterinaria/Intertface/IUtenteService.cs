using ClinicaVeterinaria.Models;

namespace ClinicaVeterinaria.Interface 
{
    public interface IUtenteService
    {
        Task<Utente> CreateUtenteAsync(Utente utente);
        Task<Utente> GetUtenteByIdAsync(int utenteId);
        Task<IEnumerable<Utente>> GetAllUtentiAsync();
        Task<Utente> UpdateUtenteAsync(Utente utente);
        Task DeleteUtenteAsync(int utenteId);
        Task<Utente> LoginAsync(string username, string password);
    }
}
