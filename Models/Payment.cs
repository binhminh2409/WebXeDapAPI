using WebXeDapAPI.Models.Enum;
using System.ComponentModel.DataAnnotations;
using System.Security;

namespace WebXeDapAPI.Models
{
    public class Payment
    {
        [Key]
        public int Id { get; set; }
        public virtual User User { get; set; }
        public virtual Order Order { get; set; }
        public decimal TotalPrice { get; set; }
        public StatusPayment Status { get; set; }
    }
}
