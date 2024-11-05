using WebXeDapAPI.Models.Enum;
using System.ComponentModel.DataAnnotations;
using System.Drawing;

namespace WebXeDapAPI.Models
{
    public class Products
    {
        [Key]
        public int Id { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public decimal PriceHasDecreased { get; set; }//giá đã giảm
        public string Description { get; set; }
        public int Quantity { get; set; }
        public string Image { get; set; }
        public DateTime Create { get; set; }
        public int BrandId { get; set; }
        public string brandName { get; set; }
        public int TypeId { get; set; }
        public string TypeName { get; set; }
        public string Colors { get; set; }
        public StatusProduct Status { get; set; }
        public ShirtSize? Size { get; set; }
    }
}
