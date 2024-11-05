using WebXeDapAPI.Models.Enum;
using System.ComponentModel.DataAnnotations;

namespace WebXeDapAPI.Models
{
    public class Ads
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime Create { get; set; }

        public string Image { get; set; }

        public int Sort { get; set; }
    }
}
