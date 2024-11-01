namespace WebXeDapAPI.Dto
{
    public class OrderDetailDto
    {
        public int Id { get; set; }
        public string OrderID { get; set; } = string.Empty;
        public int ProductID { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public decimal PriceProduc { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice { get; set; }
        public string Image { get; set; } = string.Empty;
        public string Color { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
    }
}
