namespace WebXeDapAPI.Dto
{
    public class ProductBrandInfDto
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public decimal PriceHasDecreased { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public string BrandName { get; set; }
    }
}
