using Data.Dto;
using WebXeDapAPI.Dto;
using WebXeDapAPI.Models;

namespace WebXeDapAPI.Service.Interfaces
{
    public interface ICartIService
    {
        public List<Cart> CreateBicycle(CartDto cartDto);
        public List<Cart> CreateBicycleslide(CartDtoslide cartDtoslide);
        string IncreaseQuantityShoppingCart(int UserId, int createProductId);
        object ReduceShoppingCart(int UserId, int createProductId);
        string IncreaseQuantityShoppingCartGuiId(string guiId, int createProductId);
        object ReduceShoppingCartGuiId(string guiId, int createProductId);
        bool Delete(int productId);
        List<object> GetCart(int userId);
        List<object> GetCartGuId(string GuId);
        bool DeleteCart(int userid, List<int> productIds);

    }
}
