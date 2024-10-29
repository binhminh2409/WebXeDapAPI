using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebXeDapAPI.Dto
{
    public class CommentDto
    {
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public string Description { get; set; }
    }
}
