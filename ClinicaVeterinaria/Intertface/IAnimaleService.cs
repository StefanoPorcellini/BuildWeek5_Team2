using ClinicaVeterinaria.Models;

namespace ClinicaVeterinaria.Service.Intertface
{
    public interface IAnimaleService
    {
        Task<IEnumerable<Animale>> GetAllAsync();
        Task<Animale> GetByIdAsync(int id);
        Task<Animale> CreateAsync(Animale animale);
        Task<Animale> UpdateAsync(Animale animale);
        Task DeleteAsync(int id);
        bool AnimaleExists(int id);
        Task<Proprietario> GetProprietarioByIdAsync(int proprietarioId);
        Task<Animale> SearchByChipNumberAsync(string chipNumber);
        void SaveImg(int idAnimale, IFormFile img);
    }

}
