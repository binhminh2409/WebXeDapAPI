using WebXeDapAPI.Dto;
using WebXeDapAPI.Models;

namespace WebXeDapAPI.Service.Interfaces
{
    public interface IOrderIService
    {
        public (Order, List<Order_Details>) Create(OrderDto orderDto);

        public Order_Details GetByOrderNo(string orderNo);

    }
}
