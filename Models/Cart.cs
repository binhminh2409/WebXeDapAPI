using WebXeDapAPI.Models.Enum;
using System.ComponentModel.DataAnnotations;

namespace WebXeDapAPI.Models
{
    public class Cart
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal PriceProduct { get; set; }
        public decimal TotalPrice { get; set; }
        public int Quantity { get; set; }
        public string Image { get; set; }
        public string Color { get; set; }
        public DateTime Create { get; set; }
        public StatusCart Status { get; set; }

        public List<Order_Details> Items { get; set; }
    }
}
