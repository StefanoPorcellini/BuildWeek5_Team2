using ClinicaVeterinaria.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface IClienteService
{
    Task<List<Cliente>> GetAllAsync();
    Task<Cliente> GetByIdAsync(int id);
    Task CreateAsync(Cliente cliente);
    Task UpdateAsync(Cliente cliente);
    Task<List<object>> SearchAsync(string term);
    Task<bool> ClienteExistsAsync(int id);
}
