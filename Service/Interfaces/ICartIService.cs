using Data.Dto;
using WebXeDapAPI.Dto;
using WebXeDapAPI.Models;

namespace WebXeDapAPI.Service.Interfaces
{
    public interface ICartIService
    {
        public List<Cart> CrateBicycle(CartDto cartDto);
        string IncreaseQuantityShoppingCart(int UserId,int createProductId);
        object ReduceShoppingCart(int UserId,int createProductId);
        bool Delete(int Id);
        List<object> GetCart(int userId);
        bool DeleteCart(int userid, List<int> productIds);
    }
}
