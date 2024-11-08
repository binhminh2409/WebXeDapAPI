using WebXeDapAPI.Models.Enum;
using System.ComponentModel.DataAnnotations;

namespace WebXeDapAPI.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }
        public string No_ { get; set; }
        public int UserID { get; set; }
        public string ShipName { get; set; }
        public string ShipAddress { get; set; }
        public string ShipEmail { get; set; }
        public string ShipPhone { get; set; }
        public StatusOrder Status { get; set; }
        public string? Guid { get; set; }

        public override string ToString()
        {
            return $"OrderDto [UserID={UserID}, ShipName={No_}]]";
        }

    }
}
