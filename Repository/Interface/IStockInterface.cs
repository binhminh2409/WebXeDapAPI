using WebXeDapAPI.Models;

namespace WebXeDapAPI.Repository.Interface
{
    public interface IStockInterface
    {
        Task<List<Stock>> GetAllAsync();
        
        Task<Stock> GetByIdAsync(int stockId);

        Task<Stock> CreateAsync(Stock stock);

        Task<Stock> GetByProductId(int productId);
        
        Task<Stock> DecreaseQuantity(int stockId, int decreasedBy);

        Task<Stock> IncreaseQuantity(int stockId, int increasedBy);
    }
}
