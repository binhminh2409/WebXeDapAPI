using WebXeDapAPI.Models.Enum;

namespace WebXeDapAPI.Dto
{
    public class ProductTypeInfDto
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public decimal PriceHasDecreased { get; set; }//giá đã giảm
        public string Description { get; set; }
        public string Image { get; set; }
        public string TypeName { get; set; }
        public string Colors { get; set; }

    }
}
