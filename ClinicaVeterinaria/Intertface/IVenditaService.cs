using ClinicaVeterinaria.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ClinicaVeterinaria.Service.Interface
{
    public interface IVenditaService
    {
        Task<List<Vendita>> GetAllAsync();
        Task<Vendita> GetByIdAsync(int id);
        Task CreateAsync(Vendita vendita);
        Task UpdateAsync(Vendita vendita);
        Task DeleteAsync(int id);
        Task<bool> VenditaExistsAsync(int id);
    }
}
