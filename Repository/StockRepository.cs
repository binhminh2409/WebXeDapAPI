using Microsoft.AspNetCore.Mvc.Diagnostics;
using Microsoft.EntityFrameworkCore;
using WebXeDapAPI.Data;
using WebXeDapAPI.Dto;
using WebXeDapAPI.Models;
using WebXeDapAPI.Repository.Interface;
using WebXeDapAPI.Service;
using static System.Net.Mime.MediaTypeNames;

namespace WebXeDapAPI.Repository
{
    public class StockRepository : IStockInterface
    {
        private readonly ApplicationDbContext _dbContext;
        public StockRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Stock> CreateAsync(Stock stock)
        {
            await _dbContext.Stocks.AddAsync(stock);
            await _dbContext.SaveChangesAsync();
            return stock;
        }

        public async Task<Stock> DecreaseQuantity(int stockId, int decreasedBy)
        {
            try
            {
                Stock stock = await _dbContext.Stocks.FirstOrDefaultAsync(s => s.Id == stockId);

                if (stock == null)
                {
                    throw new Exception("Stock item not found.");
                }

                if (decreasedBy < 0)
                {
                    throw new ArgumentException("Decrease amount cannot be negative.");
                }

                if (stock.Quantity < decreasedBy)
                {
                    throw new Exception("Insufficient stock quantity.");
                }

                stock.Quantity -= decreasedBy;
                await _dbContext.SaveChangesAsync();

                return stock;
            }
            catch (Exception e)
            {
                throw new Exception($"Error decreasing stock quantity: {e.Message}");
            }
        }

        public async Task<List<Stock>> GetAllAsync()
        {
            List<Stock> stocks = await _dbContext.Stocks.Include(s => s.Product).ToListAsync();
            return stocks;
        }

        public async Task<Stock> GetByIdAsync(int stockId)
        {
            Stock stock = await _dbContext.Stocks.FirstOrDefaultAsync(s => s.Id == stockId);
            return stock;
        }

        public async Task<Stock> GetByProductId(int productId)
        {
            Stock stock = await _dbContext.Stocks.Include(s => s.Product).FirstOrDefaultAsync(s => s.Product.Id == productId);
            return stock;
        }

        public async Task<Stock> IncreaseQuantity(int stockId, int increasedBy)
        {
            try
            {
                Stock stock = await _dbContext.Stocks.FirstOrDefaultAsync(s => s.Id == stockId);

                if (stock == null)
                {
                    throw new Exception("Stock item not found.");
                }

                if (increasedBy < 0)
                {
                    throw new ArgumentException("Decrease amount cannot be negative.");
                }

                if (stock.Quantity < increasedBy)
                {
                    throw new Exception("Insufficient stock quantity.");
                }

                stock.Quantity += increasedBy;
                await _dbContext.SaveChangesAsync();

                return stock;
            }
            catch (Exception e)
            {
                throw new Exception($"Error decreasing stock quantity: {e.Message}");
            }
        }

    }
}
