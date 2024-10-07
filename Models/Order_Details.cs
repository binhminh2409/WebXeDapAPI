using System.ComponentModel.DataAnnotations;

namespace WebXeDapAPI.Models
{
    public class Order_Details
    {
        [Key]
        public int Id { get; set; }
        public string OrderID { get; set; }
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public decimal PriceProduc { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice { get; set; }
        public string Image { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
