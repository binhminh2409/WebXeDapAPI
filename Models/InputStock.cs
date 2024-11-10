using WebXeDapAPI.Models.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Security.Cryptography;

namespace WebXeDapAPI.Models
{
    public class InputStock
    {
        [Key]
        public int Id { get; set; }
        public Products Product { get; set; }
        public int Quantity { get; set; }
        public DateTime CreatedTime { get; set; } = DateTime.UtcNow;
        public string Type { get; set; } = "RESTOCK";

        public int UserId { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalPrice { get; set; }
        public string BatchNo_ { get; set; }
    }
}
