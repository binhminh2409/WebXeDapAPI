using Microsoft.AspNetCore.Http;

namespace WebXeDapAPI.Dto
{
    public class ProductGetAllInfDto
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public decimal PriceHasDecreased { get; set; }//giá đã giảm
        public string Description { get; set; }
        public int Quantity { get; set; }
        public IFormFile? Image { get; set; }
        public DateTime? Create { get; set; }
        public String Status { get; set; }
    }
}
