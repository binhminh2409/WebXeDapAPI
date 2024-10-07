using WebXeDapAPI.Models;

namespace WebXeDapAPI.Repository.Interface
{
    public interface IUserInterface
    {
        User GetUser(int id);
        AccessToken GetValidTokenByUserId(int userId);
    }
}
