using WebXeDapAPI.Dto;
using WebXeDapAPI.Models;

namespace WebXeDapAPI.Repository.Interface
{
    public interface IOrderDetailsInterface
    {
        List<Order_Details> GetByUser(int userId);
        Order_Details GetByOrderId(string orderNo);

        List<Order_Details> GetAllByOrderId(string orderNo);
    }
}
