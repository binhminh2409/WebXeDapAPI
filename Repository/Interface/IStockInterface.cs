using WebXeDapAPI.Models;

namespace WebXeDapAPI.Repository.Interface
{
    public interface IStockInterface
    {
        Task<List<Stock>> GetAllAsync();

        Task<Stock> GetByIdAsync(int stockId);

        Task<Stock> CreateAsync(Stock stock);

        Stock GetByProductId(int productId);

        Task<Stock> DecreaseQuantityAsync(int stockId, int decreasedBy);

        Stock DecreaseQuantity(int stockId, int decreasedBy);


        Task<Stock> IncreaseQuantity(int stockId, int increasedBy);

        Task<Stock> GetByProductIdAsync(int productId);

    }
}
