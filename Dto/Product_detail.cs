using WebXeDapAPI.Models.Enum;

namespace WebXeDapAPI.Dto
{
    public class Product_detail
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public decimal PriceHasDecreased { get; set; }//giá đã giảm
        public string Description { get; set; }
        public string Image { get; set; }
        public string brandName { get; set; }
        public string TypeName { get; set; }
        public string Colors { get; set; }
        public string Status { get; set; }
    }

    public class ProductDetailWithColors
    {
        public Product_detail ProductDetail { get; set; }
        public List<Product_detail> ProductDetails { get; set; } // Thêm thuộc tính này
        public List<string> AvailableColors { get; set; }
    }
}
