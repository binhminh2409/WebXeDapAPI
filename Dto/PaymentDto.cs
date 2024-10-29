using System;

namespace WebXeDapAPI.Dto
{
    public class PaymentDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int OrderId { get; set; }
        public decimal TotalPrice { get; set; }
        public string Status { get; set; }

        public DateTime CreatedTime { get; set; } // Add CreatedTime
        public DateTime UpdatedTime { get; set; } // Add UpdatedTime
    }
}
