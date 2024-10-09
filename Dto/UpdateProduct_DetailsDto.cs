using Microsoft.AspNetCore.Http;

namespace WebXeDapAPI.Dto
{
    public class UpdateProduct_DetailsDto
    {
        public int ProductID { get; set; }
        public decimal Price { get; set; }
        public decimal PriceHasDecreased { get; set; }//giá đã giảm
        public IFormFile? Imgage { get; set; }
        public float Weight { get; set; } //Trọng lượng
        public string Other_Details { get; set; } //thông tin chi tiết
    }
}
