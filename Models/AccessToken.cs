using WebXeDapAPI.Models.Enum;
using System.ComponentModel.DataAnnotations;

namespace WebXeDapAPI.Models
{
    public class AccessToken
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public string? AcessToken { get; set; }
        public StatusToken Status { get; set; }
        public DateTime ExpirationDate { get; set; }
    }
}
// Test
