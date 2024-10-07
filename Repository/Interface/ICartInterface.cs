using WebXeDapAPI.Dto;
using WebXeDapAPI.Models;

namespace WebXeDapAPI.Repository.Interface
{
    public interface ICartInterface
    {
        List<GetCartInfDto> GetCartItemByUser(int userId);
        Cart GetProducId(int productId);
    }
}
