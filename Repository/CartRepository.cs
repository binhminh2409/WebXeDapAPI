using WebXeDapAPI.Data;
using WebXeDapAPI.Dto;
using WebXeDapAPI.Models;
using WebXeDapAPI.Repository.Interface;
using WebXeDapAPI.Service;
using static System.Net.Mime.MediaTypeNames;

namespace WebXeDapAPI.Repository
{
    public class CartRepository : ICartInterface
    {
        private readonly ApplicationDbContext _dbContext;
        public CartRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<GetCartInfDto> GetCartItemByUser(int userId)
        {
            return _dbContext.Carts
            .Where(x => x.UserId == userId)
            .OrderBy(x => x.UserId)
            .Select(x => new GetCartInfDto
            {
                CartId = x.Id,
                ProductId = x.ProductId,
                ProductName = x.ProductName,
                PriceProduct = x.PriceProduct,
                TotalPrice = x.TotalPrice,
                Quantity = x.Quantity,
                Image = x.Image
             })
             .ToList();
        }

        public Cart GetProducId(int productId)
        {
            return _dbContext.Carts.FirstOrDefault(x => x.ProductId == productId);
        }
    }
}
