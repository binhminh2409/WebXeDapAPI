using WebXeDapAPI.Data;
using WebXeDapAPI.Dto;
using WebXeDapAPI.Models;
using WebXeDapAPI.Repository.Interface;

namespace WebXeDapAPI.Repository
{
    public class ProductsRepository : IProductsInterface
    {
        private readonly ApplicationDbContext _dbContext;
        public ProductsRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public List<ProductBrandInfDto> GetAllBrandName(string keyword)
        {
            return _dbContext.Products
                .OrderBy(x => x.brandName)
                .Select(x => new ProductBrandInfDto
                {
                    Id = x.Id,
                    ProductName = x.ProductName,
                    Price = x.Price,
                    PriceHasDecreased = x.PriceHasDecreased,
                    Image = x.Image,
                    BrandName = x.brandName,
                })
                .ToList();
        }

        public List<ProductGetAllInfDto> GetAllProducts()
        {
            return _dbContext.Products
                 .Select(x => new ProductGetAllInfDto
                 {
                     Id = x.Id,
                     ProductName = x.ProductName,
                     Price = x.Price,
                     PriceHasDecreased = x.PriceHasDecreased,
                     Image = x.Image,
                 })
                 .ToList();
        }

        public List<ProductTypeInfDto> GetAllTypeName(string keyword, int limit = 8)
        {
            keyword = keyword.ToLower();

            // Lấy tất cả các sản phẩm có TypeName khớp với từ khóa
            var products = _dbContext.Products
                .Where(x => x.TypeName.ToLower().Contains(keyword))
                .ToList() // Chuyển đổi kết quả sang danh sách trong bộ nhớ
                .GroupBy(x => x.ProductName) // Nhóm theo tên sản phẩm
                .Select(g => g.FirstOrDefault()) // Lấy sản phẩm đầu tiên trong mỗi nhóm
                .OrderBy(x => x.TypeName)
                .Take(limit) // Lấy ra tối đa 'limit' sản phẩm
                .Select(x => new ProductTypeInfDto
                {
                    Id = x.Id,
                    ProductName = x.ProductName,
                    Price = x.Price,
                    PriceHasDecreased = x.PriceHasDecreased,
                    Image = x.Image,
                    TypeName = x.TypeName
                })
                .ToList();

            return products;
        }


        public Products GetProductsId(int Id)
        {
            return _dbContext.Products.FirstOrDefault(x => x.Id == Id);
        }

        public List<ProductGetAllInfPriceDto> GetProductsWithinPriceRangeAndBrand()
        {
            return _dbContext.Products
                 .Select(x => new ProductGetAllInfPriceDto
                 {
                     Id = x.Id,
                     ProductName = x.ProductName,
                     Price = x.Price,
                     PriceHasDecreased = x.PriceHasDecreased,
                     Image = x.Image,
                     BrandNamer = x.brandName,
                 })
                 .ToList();
        }

        public List<ProductPriceHasDecreasedInfDto> SearchProductsByPriceHasDecreased()
        {
            return _dbContext.Products
                .OrderBy(x => x.PriceHasDecreased)
                .Select(x => new ProductPriceHasDecreasedInfDto
                {
                    Id = x.Id,
                    ProductName = x.ProductName,
                    Price = x.Price,
                    PriceHasDecreased = x.PriceHasDecreased,
                    Description = x.Description,
                    Image = x.Image
                })
                .ToList();
        }
    }
}
