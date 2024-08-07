using ClinicaVeterinaria.Models;
using ClinicaVeterinaria.Models.ViewModels;

namespace ClinicaVeterinaria.Interface
{
    public interface IProdottoService
    {
        Task<IEnumerable<CasaFarmaceutica>> GetCaseFarmaceuticheAsync();
        Task<int> AddCasaFarmaceuticaAsync(CasaFarmaceutica casaFarmaceutica);
        Task AddProdottoAsync(ProdottoViewModel prodottoViewModel);

        Task<IEnumerable<Prodotto>> GetAllProdottiAsync();
        Task<Prodotto> GetProdottoByIdAsync(int id);
        Task UpdateProdottoAsync(Prodotto prodotto);
        Task DeleteProdottoAsync(int id);

        Task<CasaFarmaceutica> GetCasaFarmaceuticaByIdAsync(int id);
        Task UpdateCasaFarmaceuticaAsync(CasaFarmaceutica casaFarmaceutica);
        Task DeleteCasaFarmaceuticaAsync(int id);
        List<Prodotto> GetProdottiMemoria();
    }
}
