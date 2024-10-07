using WebXeDapAPI.Data;
using WebXeDapAPI.Dto;
using WebXeDapAPI.Models;
using WebXeDapAPI.Models.Enum;
using Microsoft.EntityFrameworkCore;
using WebXeDapAPI.Service.Interfaces;
using static System.Net.Mime.MediaTypeNames;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
using Microsoft.AspNetCore.Http;

namespace WebXeDapAPI.Service
{
    public class SlideService : ISlideIService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IConfiguration _configuration;
        public SlideService(ApplicationDbContext dbContext, IConfiguration configuration)
        {
            _configuration = configuration;
            _dbContext = dbContext;
        }
        public async Task<SlideDto> Create(SlideDto slideDto)
        {
            try
            {
                if (slideDto == null)
                {
                    throw new ArgumentNullException(nameof(slideDto), "Slide object is null or missing required information.");
                }
                if (slideDto.Image == null || slideDto.Image.Length == 0)
                {
                    throw new Exception("Image is required.");
                }
                var imagePath = await SaveImageAsync(slideDto.Image);

                Slide slide = new Slide
                {
                    Name = slideDto.Name,
                    Url = slideDto.Url,
                    Description = slideDto.Description,
                    Image = imagePath,
                    Sort = slideDto.Sort,
                };

                await _dbContext.Slides.AddAsync(slide);
                await _dbContext.SaveChangesAsync();
                SlideDto resultDto = new SlideDto
                {
                    Name = slide.Name,
                    Url = slide.Url,
                    Description = slide.Description,
                    Sort = slide.Sort
                };

                return resultDto;
            }
            catch (Exception ex)
            {
                throw new Exception("There is an error when creating a Slide", ex);
            }
        }

        public bool Delete(int Id)
        {
            try
            {
                var slide = _dbContext.Slides.FirstOrDefault(x => x.Id == Id);
                if(slide == null)
                {
                    throw new Exception("slideId not found");
                }
                _dbContext.Slides.Remove(slide);
                _dbContext.SaveChanges();
                return true;
            }
            catch(Exception ex) 
            {
                throw new Exception("There was an error while deleting the Slide",ex);
            }
        }

        public byte[] GetSileBytesImage(string image)
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

        public byte[] GetSlideBytesImageid4(string image)
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

        public byte[] GetSlideBytesImageid5(string image)
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

        public string Update(UpdateSlideDto updateSlideDto)
        {
            try
            {
                var slide = _dbContext.Slides.FirstOrDefault(x => x.Id == updateSlideDto.Id);
                if(slide == null)
                {
                    throw new Exception("slideId not found");
                }
                Slide slide1 = new Slide
                {
                    Id = slide.Id,
                    Url = updateSlideDto.Url,
                    Sort = updateSlideDto.Sort,
                };
                _dbContext.SaveChanges();
                return "Updata Successfully";
            }
            catch(Exception ex)
            {
                throw new Exception("An error occurred while updating Slides",ex);
            }
        }
        private async Task<string> SaveImageAsync(IFormFile image)
        {
            try
            {
                string currentDataFolder = DateTime.Now.ToString("dd-MM-yyyy");
                var baseFolder = _configuration.GetValue<string>("BaseAddress");

                // Tạo thư mục Product
                var productFolder = Path.Combine(baseFolder, "Slide");

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
                return Path.Combine("Slide", currentDataFolder, fileName);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while saving the image: {ex.Message}");
            }
        }
    }
}
