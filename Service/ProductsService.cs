﻿using Data.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using WebXeDapAPI.Data;
using WebXeDapAPI.Dto;
using WebXeDapAPI.Models;
using WebXeDapAPI.Models.Enum;
using WebXeDapAPI.Repository.Interface;
using WebXeDapAPI.Service.Interfaces;

namespace WebXeDapAPI.Service
{
    public class ProductsService : IProductsIService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IProductsInterface _productsInterface;
        private readonly IConfiguration _configuration;
        public ProductsService(ApplicationDbContext dbContext, IProductsInterface productsInterface, IConfiguration configuration)
        {
            _configuration = configuration;
            _productsInterface = productsInterface;
            _dbContext = dbContext;
        }
        public async Task<Products> Create(ProductDto productsDto)
        {
            try
            {
                var type = await _dbContext.Types.FindAsync(productsDto.TypeId);
                if (type == null)
                {
                    throw new Exception("typeId not found");
                }

                var brand = await _dbContext.Brands.FindAsync(productsDto.BrandId);
                if (brand == null)
                {
                    throw new Exception("brandId not found");
                }

                // Kiểm tra xem hình ảnh có hợp lệ không
                if (productsDto.image == null || productsDto.image.Length == 0)
                {
                    throw new Exception("Image is required.");
                }

                var imagePath = await SaveImageAsync(productsDto.image);

                Products products = new Products
                {
                    ProductName = productsDto.ProductName,
                    Image = imagePath,
                    Price = productsDto.Price,
                    PriceHasDecreased = productsDto.PriceHasDecreased,
                    Description = productsDto.Description,
                    Quantity = productsDto.Quantity,
                    Create = DateTime.Now,
                    TypeId = type.Id,
                    TypeName = type.Name,
                    BrandId = brand.Id,
                    brandName = brand.BrandName,
                    Colors = productsDto.Colors,
                    Status = StatusProduct.Available,
                };

                await _dbContext.Products.AddAsync(products);
                await _dbContext.SaveChangesAsync();
                return products;
            }
            catch (Exception ex)
            {
                // Log chi tiết lỗi
                throw new Exception("There is an error when creating a Product", ex);
            }
        }

        public bool Delete(int Id)
        {
            try
            {
                var delete = _dbContext.Products.FirstOrDefault(x => x.Id == Id);
                if (delete == null)
                {
                    throw new Exception("Id not found");
                }
                _dbContext.Products.Remove(delete);
                _dbContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("There was an error while deleting the Product", ex);
            }
        }

        public List<object> GetBrandName(string keyword)
        {
            keyword = keyword.ToLower();
            List<object> result = new List<object>();

            List<ProductBrandInfDto> products = _productsInterface.GetAllBrandName(keyword);
            foreach (var item in products)
            {
                var productBrandInfo = new ProductBrandInfDto
                {
                    ProductName = item.ProductName,
                    Price = item.Price,
                    PriceHasDecreased = item.PriceHasDecreased,
                    Description = item.Description,
                    Image = item.Image,
                    BrandName = item.BrandName,
                };
                result.Add(productBrandInfo);
            }
            return result;
        }

        public List<object> GetPriceHasDecreased()
        {
            List<object> result = new List<object>();

            List<ProductPriceHasDecreasedInfDto> products = _productsInterface.SearchProductsByPriceHasDecreased();
            foreach (var item in products)
            {
                var productPriceHasDecreasedInfDto = new ProductPriceHasDecreasedInfDto
                {
                    Id = item.Id,
                    ProductName = item.ProductName,
                    Price = item.Price,
                    PriceHasDecreased = item.PriceHasDecreased,
                    Description = item.Description,
                    Image = item.Image
                };
                result.Add(productPriceHasDecreasedInfDto);
            }
            return result;
        }

        public List<object> GetTypeName(string keyword, int limit = 8)
        {
            List<object> resultType = new List<object>();
            List<ProductTypeInfDto> products = _productsInterface.GetAllTypeName(keyword);
            for (int i = 0; i < Math.Min(limit, products.Count); i++)
            {
                var item = products[i];
                var productTypeInfo = new ProductTypeInfDto
                {
                    Id = item.Id,
                    ProductName = item.ProductName,
                    Price = item.Price,
                    PriceHasDecreased = item.PriceHasDecreased,
                    Description = item.Description,
                    Image = item.Image,
                    TypeName = item.TypeName,
                    Colors = item.Colors,
                };
                resultType.Add(productTypeInfo);
            }
            return resultType;
        }

        private async Task<string> SaveImageAsync(IFormFile image)
        {
            try
            {
                string currentDataFolder = DateTime.Now.ToString("dd-MM-yyyy");
                var baseFolder = _configuration.GetValue<string>("BaseAddress");

                // Tạo thư mục Product
                var productFolder = Path.Combine(baseFolder, "Product");

                if (!Directory.Exists(productFolder))
                {
                    Directory.CreateDirectory(productFolder);
                }

                // Tạo thư mục date nằm trong thư mục Product
                var folderPath = Path.Combine(productFolder, currentDataFolder);

                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
                string filePath = Path.Combine(folderPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await image.CopyToAsync(stream); // Lưu hình ảnh vào file
                }

                // Trả về tên thư mục và tên ảnh
                return Path.Combine("Product", currentDataFolder, fileName);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while saving the image: {ex.Message}");
            }
        }

        public byte[] GetProductImageBytes(string image)
        {
            try
            {
                if (string.IsNullOrEmpty(image))
                {
                    throw new FileNotFoundException("Image not found!");
                }

                var baseFolder = _configuration.GetValue<string>("BaseAddress");
                var fullPath = Path.Combine(baseFolder, image); // Tạo đường dẫn tuyệt đối

                if (!File.Exists(fullPath))
                {
                    throw new FileNotFoundException("Image not found!");
                }

                return File.ReadAllBytes(fullPath);
            }
            catch (Exception e)
            {
                throw new Exception($"An error occurred: {e.Message}");
            }
        }

        public string Update(UpdateProductDto updateProductDto, IFormFile image)
        {
            throw new NotImplementedException();
        }

        public List<ProductGetAllInfPriceDto> GetProductsWithinPriceRangeAndBrand(decimal minPrice, decimal maxPrice, string? brandsName)
        {
            List<ProductGetAllInfPriceDto> products = _productsInterface.GetProductsWithinPriceRangeAndBrand();

            List<ProductGetAllInfPriceDto> result = products
                .Where(p => p.Price >= minPrice && p.Price <= maxPrice
                             && (string.IsNullOrEmpty(brandsName) || p.BrandNamer.Equals(brandsName, StringComparison.OrdinalIgnoreCase)))
                .GroupBy(p => p.ProductName)  // Nhóm theo ProductName
                .Select(group => group.First())  // Chọn sản phẩm đầu tiên trong mỗi nhóm (loại bỏ trùng)
                .Select(p => new ProductGetAllInfPriceDto
                {
                    Id = p.Id,
                    ProductName = p.ProductName,
                    Price = p.Price,
                    PriceHasDecreased = p.PriceHasDecreased,
                    Image = p.Image,
                    BrandNamer = p.BrandNamer
                })
                .ToList();

            return result;
        }

        public ProductDetailWithColors GetProductsByNameAndColor(string productName, string? color)
        {
            var result = new ProductDetailWithColors();
            var productsQuery = _dbContext.Products.Where(p => p.ProductName == productName);
            if (!productsQuery.Any())
            {
                throw new Exception("Không tìm thấy sản phẩm nào với thông tin cung cấp.");
            }
            if (!string.IsNullOrEmpty(color))
            {
                var productWithSpecificColor = productsQuery.FirstOrDefault(p => p.Colors == color);
                if (productWithSpecificColor != null)
                {
                    result.ProductDetail = new Product_detail
                    {
                        Id = productWithSpecificColor.Id,
                        ProductName = productWithSpecificColor.ProductName,
                        Price = productWithSpecificColor.Price,
                        PriceHasDecreased = productWithSpecificColor.PriceHasDecreased,
                        Description = productWithSpecificColor.Description,
                        Image = productWithSpecificColor.Image,
                        brandName = productWithSpecificColor.brandName,
                        TypeName = productWithSpecificColor.TypeName,
                        Colors = productWithSpecificColor.Colors,
                        Status = ((StatusProduct)productWithSpecificColor.Status).ToString()
                    };
                }
                else
                {
                    result.ProductDetail = null;
                }
            }
            else
            {
                result.ProductDetails = productsQuery
                    .Select(p => new Product_detail
                    {
                        Id = p.Id,
                        ProductName = p.ProductName,
                        Price = p.Price,
                        PriceHasDecreased = p.PriceHasDecreased,
                        Description = p.Description,
                        Image = p.Image,
                        brandName = p.brandName,
                        TypeName = p.TypeName,
                        Colors = p.Colors,
                        Status = ((StatusProduct)p.Status).ToString()
                    }).ToList();
            }
            result.AvailableColors = productsQuery
                .Select(p => p.Colors)
                .Distinct()
                .ToList();

            return result;
        }
    }
}