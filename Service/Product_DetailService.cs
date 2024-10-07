using WebXeDapAPI.Data;
using WebXeDapAPI.Dto;
using WebXeDapAPI.Models;
using Microsoft.Extensions.Configuration;
using WebXeDapAPI.Service.Interfaces;
using static System.Net.Mime.MediaTypeNames;
using Microsoft.AspNetCore.Http;

namespace WebXeDapAPI.Service
{
    public class Product_DetailService : IProduct_DetailIService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IConfiguration _configuration;
        public Product_DetailService(ApplicationDbContext dbContext, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _configuration = configuration;
        }
        public async Task<Product_Details> Create(Product_DetailDto product_DetailDto)
        {
            try
            {
                var products = _dbContext.Products.SingleOrDefault(x => x.Id == product_DetailDto.ProductID);

                if (products == null)
                {
                    throw new Exception("Product not found");
                }

                // Tạo danh sách lưu trữ đường dẫn hình ảnh
                List<string> imagePaths = new List<string>();

                // Lưu từng ảnh và thêm đường dẫn vào danh sách
                foreach (var image in product_DetailDto.Imgage)
                {
                    var imagePath = await SaveImageAsync(image);
                    imagePaths.Add(imagePath);
                }

                Product_Details product_Details = new Product_Details
                {
                    ProductID = products.Id,
                    BrandId = products.BrandId,
                    Price = products.Price,
                    PriceHasDecreased = products.PriceHasDecreased,
                    Imgage = string.Join(";", imagePaths), // Kết hợp các đường dẫn thành một chuỗi
                    Weight = product_DetailDto.Weight,
                    Other_Details = product_DetailDto.Other_Details,
                };

                _dbContext.Product_Details.Add(product_Details);
                _dbContext.SaveChanges();
                return product_Details;
            }
            catch (Exception ex)
            {
                throw new Exception("There is an error when creating a Product_Detail", ex);
            }
        }

        public bool Delete(int Id)
        {
            try
            {
                var query = _dbContext.Product_Details.FirstOrDefault(x => x.Id == Id);
                if (query == null)
                {
                    throw new Exception("Product_DetailsId not found");
                }
                _dbContext.Remove(query);
                _dbContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("There was an error while deleting the Product_Detail", ex);
            }
        }

        public string Update(UpdateProduct_DetailsDto updateProduct_DetailsDto)
        {
            try
            {
                var product_Details = _dbContext.Product_Details.FirstOrDefault(x => x.Id == updateProduct_DetailsDto.Id);
                if (product_Details == null)
                {
                    throw new Exception("Product_DetailId not found");
                }
                product_Details.Weight = updateProduct_DetailsDto.Weight;
                product_Details.Other_Details = updateProduct_DetailsDto.Other_Details;
                _dbContext.Product_Details.Update(product_Details);
                _dbContext.SaveChanges();
                return "Update Successfully";
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while updating Product_Details", ex);
            }
        }
        private async Task<string> SaveImageAsync(IFormFile image)
        {
            try
            {
                string currentDataFolder = DateTime.Now.ToString("dd-MM-yyyy");
                var baseFolder = _configuration.GetValue<string>("BaseAddress");
                var productFolder = Path.Combine(baseFolder, "Product_Detail");
                if (!Directory.Exists(productFolder))
                {
                    Directory.CreateDirectory(productFolder);
                }
                var folderPath = Path.Combine(productFolder, currentDataFolder);
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
                string filePath = Path.Combine(folderPath, fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await image.CopyToAsync(stream);
                }
                return $"Product_Detail/{currentDataFolder}/{fileName}";
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while saving the image: {ex.Message}");
            }
        }

    }
}
