using WebXeDapAPI.Dto;
using WebXeDapAPI.Models;

namespace WebXeDapAPI.Repository.Interface
{
    public interface ICartInterface
    {
        List<GetCartInfDto> GetCartItemByUser(int userId);
        List<GetCartInfDto> GetCartItemByGuId(string GuId);
        Cart GetProducId(int productId);
    }
}
