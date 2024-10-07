using WebXeDapAPI.Dto;
using WebXeDapAPI.Models;

namespace WebXeDapAPI.Repository.Interface
{
    public interface IProducts_DrtailInterface
    {
        public ProductDetailGetInfDto Getproducts_Detail(int productId);
    }
}
