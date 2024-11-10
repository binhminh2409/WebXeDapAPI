using WebXeDapAPI.Dto;
using WebXeDapAPI.Models;

namespace WebXeDapAPI.Repository.Interface
{
    public interface IStockIService
    {
        Task<List<StockDto>> GetAllAsync();
        
        Task<StockDto> GetByIdAsync(int stockId);

        Task<StockDto> CreateAsync(StockDto stock);

        Task<StockDto> GetByProductId(int productId);
        
        Task<StockDto> DecreaseQuantity(int stockId, int decreasedBy);

        Task<StockDto> IncreaseQuantity(int stockId, int increasedBy);

        Task<List<StockDto>> DecreaseQuantityByOrderWithDetail(OrderWithDetailDto orderWithDetailDto);

        Task<List<InputStockDto>> RestockOrder(List<InputStockDto> inputStockDtos);

        Task<List<InputStockDto>> RestockOrderUpdateStatus(string batchNo_, string status);

        Task<List<InputStockDto>> RestockHistory();


    }
}
