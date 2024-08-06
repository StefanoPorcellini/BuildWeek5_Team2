using ClinicaVeterinaria.Models;

namespace ClinicaVeterinaria.Service.Intertface
{
    public interface IVisitaService
    {
        Task<IEnumerable<Visita>> GetAllByAnimaleIdAsync(int animaleId);
        Task<Visita> GetByIdAsync(int id);
        Task<Visita> CreateAsync(Visita visita);
        Task<Visita> UpdateAsync(Visita visita);
        Task DeleteAsync(Visita visita);
        bool VisitaExists(int id);
    }

}
