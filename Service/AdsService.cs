using Data.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebXeDapAPI.Data;
using WebXeDapAPI.Dto;
using WebXeDapAPI.Models;
using WebXeDapAPI.Models.Enum;
using WebXeDapAPI.Service.Interfaces;

namespace WebXeDapAPI.Service
{
    public class AdsService : IAdsIService
    {
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _dbContext;
        public AdsService(IConfiguration configuration, ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            _configuration = configuration;
        }
        public async Task<Ads> Create (AdsDto adsDto)
        {
            try
            {
                if (adsDto == null)
                {
                    throw new ArgumentNullException(nameof(adsDto), "Brand object is null or missing required information.");
                }
                if (adsDto.Image == null || adsDto.Image.Length == 0)
                {
                    throw new Exception("Image is required.");
                }

                var imagePath = await SaveImageAsync(adsDto.Image);

                var ads = new Ads
                {
                    Name = adsDto.Name,
                    CreateDate = DateTime.Now,
                    Image = imagePath,
                    Url = adsDto.Url,
                    Sort = adsDto.Sort
                };
                _dbContext.Ads.Add(ads);
                _dbContext.SaveChanges();
                return ads;
            }
            catch (Exception ex) 
            {
                throw new Exception("There is an error when creating a Ads", ex);
            }
        }

        public async Task<Ads> Delete(int id)
        {
            try
            {
                if (id == 0)
                {
                    throw new Exception("Id not found");
                }

                var ads = await _dbContext.Ads.FirstOrDefaultAsync(x => x.Id == id);
                if (ads == null)
                {
                    throw new Exception("Ads not found");
                }

                _dbContext.Remove(ads);
                await _dbContext.SaveChangesAsync();

                return ads;
            }
            catch (Exception ex)
            {
                throw new Exception("There is an error when deleting an Ads", ex);
            }
        }

        public byte[] GetAdsBytesImage(string imagePath)
        {
            throw new NotImplementedException();
        }

        public ICollection<Ads> GetAll()
        {
            var ads = _dbContext.Ads
                                .OrderBy(x => x.Sort)
                                .Select(x => new Ads
                                {
                                    Id = x.Id,
                                    Name = x.Name,
                                    Image = x.Image,
                                    Url = x.Url,
                                    Sort = x.Sort,
                                })
                                .ToList();

            return ads;
        }

        public Task<Ads> Update(Ads ads)
        {
            throw new NotImplementedException();
        }

        private async Task<string> SaveImageAsync(IFormFile Image)
        {
            try
            {
                string currentDataFolder = DateTime.Now.ToString("dd-MM-yyyy");
                var baseFolder = _configuration.GetValue<string>("BaseAddress");
                var productFolder = Path.Combine(baseFolder, "Ads");
                if (!Directory.Exists(productFolder))
                {
                    Directory.CreateDirectory(productFolder);
                }
                var folderPath = Path.Combine(productFolder, currentDataFolder);
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(Image.FileName);
                string filePath = Path.Combine(folderPath, fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await Image.CopyToAsync(stream);
                }
                return $"Ads/{currentDataFolder}/{fileName}";
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while saving the image: {ex.Message}");
            }
        }
    }
}
