using WebXeDapAPI.Models.Enum;

namespace WebXeDapAPI.Dto
{
    public class OrderDto
    {
        public int UserID { get; set; }
        public string ShipName { get; set; }
        public string ShipAddress { get; set; }
        public string ShipEmail { get; set; }
        public string ShipPhone { get; set; }
        public List<int> Cart { get; set; }
    }
}
