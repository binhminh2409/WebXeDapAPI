using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebXeDapAPI.Dto
{
    public class RevenueRequestDto
    {
        public DateTime StartDate { get; set; }  // Ngày bắt đầu
        public DateTime EndDate { get; set; }    // Ngày kết thúc
    }
}
