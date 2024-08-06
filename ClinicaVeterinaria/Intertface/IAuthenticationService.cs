using ClinicaVeterinaria.Models;

namespace ClinicaVeterinaria.Service
{
    public interface IAuthenticationService
    {
        Task SignInAsync(Utente user);
        Task SignOutAsync();
    }
}
