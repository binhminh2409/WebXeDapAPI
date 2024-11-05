using WebXeDapAPI.Models.Enum;
using System.ComponentModel.DataAnnotations;

namespace WebXeDapAPI.Models
{
    public class Stock
    {
        [Key]
        public int Id { get; set; }
        public Products Product { get; set; }
        public int Quantity { get; set; }
        public DateTime UpdatedTime { get; set; } = DateTime.UtcNow; 
    }
}