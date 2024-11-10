using WebXeDapAPI.Models.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Data.Dto;

namespace WebXeDapAPI.Models
{
    public class InputStockDto
    {
        public int? Id { get; set; }
        public ProductDto? Product { get; set; }
        public int? ProductId { get; set; }
        public int Quantity { get; set; }
        public DateTime CreatedTime { get; set; } = DateTime.UtcNow;
        public string Type { get; set; }

        public string Status { get; set; }
        public bool Paid { get; set; } = false;
        public string? ReturnReason { get; set; }

        public int UserId { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalPrice { get; set; }

        public string? BatchNo_ { get; set; }
    }
}
