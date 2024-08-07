using ClinicaVeterinaria.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ClinicaVeterinaria.Interface
{
    public interface IRicoveroService
    {
        Task<List<Ricovero>> GetAllAsync();
        Task<Ricovero> GetByIdAsync(int id);
        Task<Ricovero> GetRicoveroWithAnimaleAsync(int id);
        Task<decimal> CalcolaCostoTotaleAsync(int ricoveroId, DateTime? dataFine = null);
        Task CreateAsync(Ricovero ricovero);
        Task UpdateAsync(Ricovero ricovero);
        Task DeleteAsync(int id);
        Task<List<Ricovero>> GetRicoveriAttiviAsync();
    }
}
