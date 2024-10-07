using WebXeDapAPI.Data;
using WebXeDapAPI.Dto;
using WebXeDapAPI.Service.Interfaces;
using Type = WebXeDapAPI.Models.Type;

namespace WebXeDapAPI.Service
{
    public class TypeService : ITypeIService
    {
        private readonly ApplicationDbContext _dbContext;
        public TypeService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public Type Create(Type type)
        {
            try
            {
                if (type == null)
                {
                    throw new ArgumentNullException(nameof(type), "Type object is null or missing required information.");
                }
                _dbContext.Types.Add(type);
                _dbContext.SaveChanges();
                return type;
            }
            catch (Exception ex)
            {
                throw new Exception("There is an error when creating a Type", ex);
            }
        }

        public bool Delete(int typeId)
        {
            try
            {
                var typeToDelete = _dbContext.Types.FirstOrDefault(x => x.Id == typeId);
                if (typeToDelete == null)
                {
                    throw new Exception("Type not found");
                }
                _dbContext.Types.Remove(typeToDelete);
                _dbContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("There was an error while deleting the Type", ex);
            }
        }


        public string Update(TypeDto typeDto)
        {
            try
            {
                var query = _dbContext.Types.FirstOrDefault(x => x.Id == typeDto.Id);
                if (query == null)
                {
                    throw new Exception("Type not found");
                }

                query.Description = typeDto.Description;

                _dbContext.Types.Update(query);
                _dbContext.SaveChanges();
                return "Update Successfully";
            }
            catch (Exception ex)
            {
                throw new Exception("There was an error while updating Type", ex);
            }
        }
    }
}
