using WebXeDapAPI.Data;
using WebXeDapAPI.Dto;
using WebXeDapAPI.Models;
using Microsoft.EntityFrameworkCore.Internal;
using WebXeDapAPI.Repository.Interface;
using static System.Net.Mime.MediaTypeNames;

namespace WebXeDapAPI.Repository
{
    public class Products_DetailRepository : IProducts_DrtailInterface
    {
        private readonly ApplicationDbContext _dbContext;
        public Products_DetailRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public ProductDetailGetInfDto Getproducts_Detail(int productId)
        {
            var productDetails = _dbContext.Product_Details.FirstOrDefault(p => p.ProductID == productId);
            if (productDetails == null)
            {
                Console.WriteLine("Không tìm thấy dữ liệu trong Product_Details với ProductID: " + productId);
                return null;
            }

            var result = (from productDetail in _dbContext.Product_Details
                          join brand in _dbContext.Brands on productDetail.BrandId equals brand.Id
                          join product in _dbContext.Products on productDetail.ProductID equals product.Id
                          where productDetail.ProductID == productId
                          select new ProductDetailGetInfDto
                          {
                              Id = productDetail.Id,
                              ProductID = productDetail.ProductID,
                              Price = productDetail.Price,
                              ProductName = product.ProductName,
                              PriceHasDecreased = productDetail.PriceHasDecreased,
                              BrandName = brand.BrandName,
                              Imgage = productDetail.Imgage,
                              Weight = productDetail.Weight,
                              Other_Details = productDetail.Other_Details
                          }).FirstOrDefault();

            if (result == null)
            {
                Console.WriteLine("Không có dữ liệu sau khi join.");
            }

            return result;
        }
    }
}
