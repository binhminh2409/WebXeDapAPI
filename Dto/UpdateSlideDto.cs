using WebXeDapAPI.Models.Enum;

namespace WebXeDapAPI.Dto
{
    public class UpdateSlideDto
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public int Sort { get; set; }
    }
}
