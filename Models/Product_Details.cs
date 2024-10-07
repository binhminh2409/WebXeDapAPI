using System.ComponentModel.DataAnnotations;

namespace WebXeDapAPI.Models
{
    public class Product_Details
    {
        [Key]
        public int Id { get; set; }
        public int ProductID { get; set; }
        public decimal Price { get; set; }
        public decimal PriceHasDecreased { get; set; }//giá đã giảm
        public int BrandId { get; set; } // thương hiệu
        public string Imgage { get; set; }
        public float Weight { get; set; } //Trọng lượng
        public string Other_Details { get; set; } //thông tin chi tiết
    }
}
