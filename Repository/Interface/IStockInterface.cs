using WebXeDapAPI.Models;

namespace WebXeDapAPI.Repository.Interface
{
    public interface IStockInterface
    {
        Task<List<Stock>> GetAll();
        
        Task<Stock> GetByIdAsync(int stockId);

        Task<Stock> CreateAsync(Stock stock);

        Task<Stock> GetByProductId(int productId);

        Task<Stock> Update(Stock stock);

    }
}
