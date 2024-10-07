using Microsoft.AspNetCore.Http;

namespace WebXeDapAPI.Dto
{
    public class Product_DetailDto
    {
        public int ProductID { get; set; }
        public List<IFormFile> Imgage { get; set; }
        public float Weight { get; set; } //Trọng lượng
        public string Other_Details { get; set; } //thông tin chi tiết
    }
}
