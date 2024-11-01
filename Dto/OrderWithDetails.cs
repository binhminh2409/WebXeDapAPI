namespace WebXeDapAPI.Dto
{
    public class OrderWithDetailDto
    {
        public int? Id { get; set; }
        public int? UserID { get; set; }
        public string ShipName { get; set; } = string.Empty;
        public string ShipAddress { get; set; } = string.Empty;
        public string ShipEmail { get; set; } = string.Empty;
        public string ShipPhone { get; set; } = string.Empty;
        public List<int> Cart { get; set; } = new List<int>();
        public string No_ { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public OrderDetailDto OrderDetails { get; set; } = new OrderDetailDto();
    }
}
