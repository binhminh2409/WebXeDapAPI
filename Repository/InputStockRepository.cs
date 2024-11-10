using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebXeDapAPI.Models;
using WebXeDapAPI.Data;
using WebXeDapAPI.Service.Interfaces;
using System.Runtime.CompilerServices; // Adjust according to your project structure

namespace WebXeDapAPI.Service.Implementations
{
    public class InputStockRepository : IInputStockInterface
    {
        private readonly ApplicationDbContext _dbContext;

        public InputStockRepository(ApplicationDbContext context)
        {
            _dbContext = context;
        }

        public async Task<InputStock> CreateAsync(InputStock inputStock)
        {
            _dbContext.InputStocks.Add(inputStock);
            await _dbContext.SaveChangesAsync();

            // Reload the stock entity including the Product
            var createdStockInput = await _dbContext.InputStocks
                .Include(s => s.Product) // Include the Product navigation property
                .FirstOrDefaultAsync(s => s.Id == inputStock.Id); // Get the created stock by Id
            return createdStockInput;
        }

        public async Task<InputStock> GetByIdAsync(int id)
        {
            return await _dbContext.InputStocks.FindAsync(id);
        }

        public async Task<List<InputStock>> GetAllAsync()
        {
            return await _dbContext.InputStocks.Include(s => s.Product).ToListAsync();
        }

        public async Task<InputStock> UpdateAsync(InputStock inputStock)
        {
            _dbContext.InputStocks.Update(inputStock);
            await _dbContext.SaveChangesAsync();
            return inputStock;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var inputStock = await GetByIdAsync(id);
            if (inputStock == null)
            {
                return false;
            }
            _dbContext.InputStocks.Remove(inputStock);
            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}
