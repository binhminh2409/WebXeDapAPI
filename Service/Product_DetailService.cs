using WebXeDapAPI.Data;
using WebXeDapAPI.Dto;
using WebXeDapAPI.Models;
using Microsoft.Extensions.Configuration;
using WebXeDapAPI.Service.Interfaces;
using static System.Net.Mime.MediaTypeNames;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

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
                // Kiểm tra ID sản phẩm có tồn tại không
                var product = await _dbContext.Products.SingleOrDefaultAsync(x => x.Id == product_DetailDto.ProductID);

                if (product == null)
                {
                    throw new Exception("Product not found");
                }

                // Danh sách lưu trữ đường dẫn hình ảnh
                List<string> imagePaths = new List<string>();

                // Lưu từng ảnh và thêm đường dẫn vào danh sách
                foreach (var image in product_DetailDto.Imgage)
                {
                    try
                    {
                        var imagePath = await SaveImageAsync(image);
                        imagePaths.Add(imagePath);
                    }
                    catch (Exception ex)
                    {
                        // Ghi lại lỗi trong khi lưu hình ảnh
                        throw new Exception($"Error saving image: {ex.Message}", ex);
                    }
                }

                // Tạo đối tượng Product_Details
                Product_Details product_Details = new Product_Details
                {
                    ProductID = product.Id,
                    BrandId = product.BrandId,
                    Price = product.Price,
                    PriceHasDecreased = product.PriceHasDecreased,
                    Imgage = string.Join(";", imagePaths), // Kết hợp các đường dẫn thành một chuỗi
                    Weight = product_DetailDto.Weight,
                    Other_Details = product_DetailDto.Other_Details,
                };

                // Thêm vào DbContext và lưu thay đổi
                _dbContext.Product_Details.Add(product_Details);
                await _dbContext.SaveChangesAsync(); // Thay đổi sang async
                return product_Details;
            }
            catch (Exception ex)
            {
                // Ghi lại chi tiết lỗi
                // Bạn có thể sử dụng một logger để ghi lại thông tin lỗi ở đây
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

        public async Task<UpdateProduct_DetailsDto> Update(int Id, UpdateProduct_DetailsDto updateProduct_DetailsDto)
        {
            try
            {
                var product_Details = await _dbContext.Product_Details.FirstOrDefaultAsync(x => x.Id == Id);
                if (product_Details == null)
                {
                    throw new Exception("Product_DetailId not found");
                }

                // Cập nhật ProductID nếu có giá trị hợp lệ
                if (updateProduct_DetailsDto.ProductID > 0)
                {
                    product_Details.ProductID = updateProduct_DetailsDto.ProductID;
                }

                if (updateProduct_DetailsDto.Weight > 0)
                {
                    product_Details.Weight = updateProduct_DetailsDto.Weight;
                }

                if (!string.IsNullOrEmpty(updateProduct_DetailsDto.Other_Details) && updateProduct_DetailsDto.Other_Details != "null")
                {
                    product_Details.Other_Details = updateProduct_DetailsDto.Other_Details;
                }

                if (updateProduct_DetailsDto.Price > 0)
                {
                    product_Details.Price = updateProduct_DetailsDto.Price;
                }

                if (updateProduct_DetailsDto.PriceHasDecreased >= 0)
                {
                    product_Details.PriceHasDecreased = updateProduct_DetailsDto.PriceHasDecreased;
                }

                if (updateProduct_DetailsDto.Imgage != null)
                {
                    product_Details.Imgage = await SaveImageAsync(updateProduct_DetailsDto.Imgage);
                }

                _dbContext.Product_Details.Update(product_Details);
                await _dbContext.SaveChangesAsync();

                var updatedDto = new UpdateProduct_DetailsDto
                {
                    ProductID = product_Details.ProductID,
                    Weight = product_Details.Weight,
                    Other_Details = product_Details.Other_Details,
                    Price = product_Details.Price,
                    PriceHasDecreased = product_Details.PriceHasDecreased,
                };

                return updatedDto;
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
