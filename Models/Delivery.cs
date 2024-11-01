using WebXeDapAPI.Models.Enum;
using System.ComponentModel.DataAnnotations;

namespace WebXeDapAPI.Models
{
    public class Delivery
    {
        [Key]
        public int Id { get; set; }

        public int UserId { get; set; }
        
        public string No_ { get; set; }

        public StatusDelivery Status { get; set; }

        public Payment? Payment { get; set; }

        public DateTime CreatedTime { get; set; } = DateTime.UtcNow; 
        
        public DateTime UpdatedTime { get; set; } = DateTime.UtcNow; 

    }
}
