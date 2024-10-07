using WebXeDapAPI.Models.Enum;
using System.ComponentModel.DataAnnotations;

namespace WebXeDapAPI.Models
{
    public class Slide
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }
        public decimal PriceHasDecreased { get; set; }
        public string Image { get; set; }
        public int Sort { get; set; }
        public StatusSlide Status { get; set; }
    }
}
