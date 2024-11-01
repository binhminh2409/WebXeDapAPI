using WebXeDapAPI.Models.Enum;

namespace WebXeDapAPI.Dto
{
    public class GetCartInfDto
    {
        public int CartId { get; set; }
        public int ProductId { get; set; }
        public decimal PriceProduct { get; set; }
        public string ProductName { get; set; }
        public decimal TotalPrice { get; set; }
        public int Quantity { get; set; }
        public string Image { get; set; }
        public string GuId { get; set; }
    }
}
