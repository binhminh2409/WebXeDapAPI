using WebXeDapAPI.Data;
using WebXeDapAPI.Models;
using WebXeDapAPI.Repository.Interface;

namespace WebXeDapAPI.Repository
{
    public class UserRepository : IUserInterface
    {
        private readonly ApplicationDbContext _dbContext;
        public UserRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public User GetUser(int id)
        {
            throw new NotImplementedException();
        }

        public AccessToken GetValidTokenByUserId(int userId)
        {
            return _dbContext.AccessTokens.FirstOrDefault(x => x.Id == userId);
        }
    }
}
