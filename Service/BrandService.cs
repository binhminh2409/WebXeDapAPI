using WebXeDapAPI.Data;
using WebXeDapAPI.Dto;
using WebXeDapAPI.Models;
using WebXeDapAPI.Service.Interfaces;

namespace WebXeDapAPI.Service
{
    public class BrandService : IBrandIService
    {
        private readonly ApplicationDbContext _dbContext;
        public BrandService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public Brand Create(Brand brand)
        {
            try
            {
                if(brand == null)
                {
                    throw new ArgumentNullException(nameof(brand), "Brand object is null or missing required information.");
                }
                _dbContext.Brands.Add(brand);
                _dbContext.SaveChanges();
                return brand;
            }
            catch (Exception ex)
            {
                throw new Exception("There is an error when creating a Brand", ex);
            }
        }

        public bool Delete(int id)
        {
            try
            {
                var brand = _dbContext.Brands.FirstOrDefault(x => x.Id == id);
                if(brand == null)
                {
                    throw new Exception("brandId not found");
                }
                _dbContext.Remove(brand);
                _dbContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("There was an error while deleting the Brand");
            }
        }

        public string Update(BrandDto brandDto)
        {
            try
            {
                var query = _dbContext.Brands.FirstOrDefault(x => x.Id == brandDto.Id);
                if(query == null)
                {
                    throw new Exception("BrandId not found");
                }
                query.Origin = brandDto.Origin;
                _dbContext.Update(query);
                _dbContext.SaveChanges();
                return "update Successfully";
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while updating Brand", ex);
            }
        }

        public Brand getById(int id) {
            return _dbContext.Brands.Find(id);
        }
    }
}
