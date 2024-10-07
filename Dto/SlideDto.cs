using Microsoft.AspNetCore.Http;
using WebXeDapAPI.Models.Enum;

namespace WebXeDapAPI.Dto
{
    public class SlideDto
    {
        public string Name { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }
        public IFormFile Image { get; set; }
        public int Sort { get; set; }
    }
}
