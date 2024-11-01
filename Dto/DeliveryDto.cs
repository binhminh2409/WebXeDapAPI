using System.ComponentModel.DataAnnotations;
using WebXeDapAPI.Dto;

namespace WebXeDapAPI.Models
{
    public class DeliveryDto
    {
        [Key]
        public int Id { get; set; }

        public int UserId { get; set; }

        public string No_ { get; set; }
        
        public string Status { get; set; }

        public PaymentDto? Payment { get; set; }

        public DateTime CreatedTime { get; set; } = DateTime.UtcNow; 
        
        public DateTime UpdatedTime { get; set; } = DateTime.UtcNow; 

    }
}
