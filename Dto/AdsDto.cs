using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebXeDapAPI.Dto
{
    public class AdsDto
    {
        public string Name { get; set; }
        public IFormFile Image { get; set; }
        public string Url { get; set; }
        public int Sort { get; set; }
    }
}
