using System;
using WebXeDapAPI.Models.Enum;
using System.ComponentModel.DataAnnotations;

namespace WebXeDapAPI.Models
{
    public class Payment
    {
        [Key]
        public int Id { get; set; }
        public User User { get; set; }
        public Order Order { get; set; }
        public decimal TotalPrice { get; set; }
        public StatusPayment Status { get; set; }
        public decimal ExtraFee { get; set; }
        public Method Method { get; set; }
        public DateTime CreatedTime { get; set; } = DateTime.UtcNow; 
        public DateTime UpdatedTime { get; set; } = DateTime.UtcNow; 
    }
}
