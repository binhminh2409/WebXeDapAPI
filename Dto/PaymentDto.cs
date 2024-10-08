using WebXeDapAPI.Models.Enum;
using System.ComponentModel.DataAnnotations;
using System.Security;

namespace WebXeDapAPI.Dto
{
    public class PaymentDto
    {
        [Key]
        public int Id { get; set; }
        public int UserID { get; set; }
        public int OrderID { get; set; }
        public string ShipName { get; set; }
        public string ShipAddress { get; set; }
        public string ShipEmail { get; set; }
        public string ShipPhone { get; set; }
        public decimal TotalPrice { get; set; }
        public string Status { get; set; }
    }
}
