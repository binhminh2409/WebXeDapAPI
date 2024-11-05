using WebXeDapAPI.Models.Enum;
using System.ComponentModel.DataAnnotations;
using Data.Dto;

namespace WebXeDapAPI.Models
{
    public class StockDto
    {
        public int Id { get; set; }
        public ProductDto? Product { get; set; }
        public int productId  { get; set; } = 0;
        public int Quantity { get; set; }
        public DateTime UpdatedTime { get; set; } = DateTime.UtcNow; 
    }
}
