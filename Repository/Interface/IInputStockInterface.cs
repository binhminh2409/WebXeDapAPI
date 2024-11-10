using System.Collections.Generic;
using System.Threading.Tasks;
using WebXeDapAPI.Models;

namespace WebXeDapAPI.Service.Interfaces
{
    public interface IInputStockInterface
    {
        Task<InputStock> CreateAsync(InputStock inputStock);
        Task<InputStock> GetByIdAsync(int id);
        Task<List<InputStock>> GetAllAsync();
        Task<InputStock> UpdateAsync(InputStock inputStock);
        Task<bool> DeleteAsync(int id);
    }
}
