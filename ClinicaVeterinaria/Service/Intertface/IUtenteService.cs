using ClinicaVeterinaria.Models;

namespace ClinicaVeterinaria.Service.Intertface
{
    public interface IUtenteService
    {
        Task<Utente> CreateUtenteAsync(Utente utente);
        Task<Utente> GetUtenteByIdAsync(int utenteId);
        Task<IEnumerable<Utente>> GetAllUtentiAsync();
        Task<Utente> UpdateUtenteAsync(Utente utente);
        Task DeleteUtenteAsync(int utenteId);
        Utente Login(string username, string password);
    }
}
