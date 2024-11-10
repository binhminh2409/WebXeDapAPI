using WebXeDapAPI.Dto;
using WebXeDapAPI.Models;

namespace WebXeDapAPI.Repository.Interface
{
    public interface IOrderInterface
    {
        List<Order> GetByUser(int userId);
        Order GetById(int orderId);

        List<Order> GetByGuid(string Guid);

        Order Update(Order order);
    }
}
