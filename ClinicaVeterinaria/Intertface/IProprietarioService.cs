using ClinicaVeterinaria.Models;

namespace ClinicaVeterinaria.Service.Intertface
{
    public interface IProprietarioService
    {
        Task<IEnumerable<Proprietario>> GetAllAsync();
        Task<Proprietario> GetByIdAsync(int id);
        Task<Proprietario> CreateAsync(Proprietario proprietario);
        Task<Proprietario> UpdateAsync(Proprietario proprietario);
        Task DeleteAsync(int id);
        Task<IEnumerable<Proprietario>> SearchAsync(string term);
    }
}