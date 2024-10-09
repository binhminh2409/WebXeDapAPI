using WebXeDapAPI.Models.Enum;
using System.ComponentModel.DataAnnotations;
using System.Security;

namespace WebXeDapAPI.Dto
{
    public class PaymentDto
    {
        [Key]
        public int Id { get; set; }
        public UserDto User { get; set; }
        public OrderDto Order { get; set; }
        public decimal TotalPrice { get; set; }
        public string Status { get; set; }
    }
}
