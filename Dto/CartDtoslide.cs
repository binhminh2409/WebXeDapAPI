using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebXeDapAPI.Dto
{
    public class CartDtoslide
    {
        public int? UserId { get; set; }
        public string? GuiId { get; set; }
        public List<int> ProductIDs { get; set; }
    }
}
