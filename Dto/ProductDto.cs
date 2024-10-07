using Microsoft.AspNetCore.Http;
using WebXeDapAPI.Models;

namespace Data.Dto
{
    public class ProductDto
    {
        public string ProductName { get; set; }
        public IFormFile image { get; set; }
        public decimal Price { get; set; }
        public decimal PriceHasDecreased { get; set; } // Giá đã giảm
        public string Description { get; set; }
        public int Quantity { get; set; }
        public int BrandId { get; set; }
        public int TypeId { get; set; }
        public string Colors { get; set; }
    }
}
