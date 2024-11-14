using WebXeDapAPI.Dto;
using WebXeDapAPI.Models;

namespace WebXeDapAPI.Service.Interfaces
{
    public interface IOrderIService
    {
        public (Order, List<Order_Details>) Create(OrderDto orderDto);
        public Order_Details GetByOrderNo(string orderNo);

        public string CancelOrder(int orderId);

        public List<OrderDto> GetByUser(int userId);

        public List<OrderWithDetailDto> GetByUserWithDetail(int userId);

        public List<OrderWithDetailDto> GetByGuid(string Guid);

        public List<Order_Details> GetDetailsByUser(int userId);

        public OrderWithDetailDto GetByIdWithDetail(int orderId);
        

        public Task<List<ProductGetAllInfPriceDto>> ListOfBestSellingProducts();
    }
}
