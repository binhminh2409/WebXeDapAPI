using WebXeDapAPI.Models.Enum;

namespace Data.Dto
{
    public class CartDto
    {
        public int? UserId { get; set; }
        public string? GuiId { get; set; }
        public List<int> ProductIDs { get; set; }

    }
}
