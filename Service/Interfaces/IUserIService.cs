using WebXeDapAPI.Dto;
using WebXeDapAPI.Models;

namespace WebXeDapAPI.Service.Interfaces
{
    public interface IUserIService
    {
        public UserDto RegisterUser(UserDto userdto);
        public UserDto RegisterUserAdmin(UserDto userdto);
        User Login(RequestDto requetDto);
        bool logout (int UserId);
        string ResetPassword(int UserId);
        bool CheckIsBirthday(User user);
    }
}
