namespace WebXeDapAPI.Dto
{
    public class UpdateProductDto
    {
        public int Id { get; set; }
        public decimal Price { get; set; }
        public decimal PriceHasDecreased { get; set; }//giá đã giảm
        public string Description { get; set; }
    }
}
