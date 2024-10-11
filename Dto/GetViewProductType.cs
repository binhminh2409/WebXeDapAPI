using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebXeDapAPI.Dto
{
    public class GetViewProductType
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public decimal PriceHasDecreased { get; set; }//giá đã giảm
        public string Description { get; set; }
        public string Image { get; set; }
        public int BrandId { get; set; }
        public string brandName { get; set; }
        public int TypeId { get; set; }
        public string TypeName { get; set; }
        public string Colors { get; set; }
        public string ProductType_ProductType { get; set; }
    }
}
