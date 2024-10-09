using WebXeDapAPI.Dto;
using WebXeDapAPI.Models;

namespace WebXeDapAPI.Service.Interfaces
{
    public interface IOrderIService
    {
        public (Order, List<Order_Details>) Create(OrderDto orderDto);
        decimal CalculateRevenue(DateTime startDate, DateTime endDate);
        decimal CalculateRevenueByType(int typeId, DateTime startDate, DateTime endDate);
    }
}
