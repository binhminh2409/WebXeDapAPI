using Microsoft.AspNetCore.Http;
using WebXeDapAPI.Dto;
using WebXeDapAPI.Models;

namespace WebXeDapAPI.Service.Interfaces
{
    public interface IUserIService
    {
        public UserDto RegisterUser(UserDto userdto);
        public UserDto RegisterUserAdmin(UserDto userdto);
        User Login(RequestDto requetDto);
        bool logout(int UserId);
        string ResetPassword(int UserId);
        Task<GetViewUser> GetUser(int UserId);
        Task<UpdateGetViewUser> UpdateViewUser(int userId, UpdateGetViewUser updateUserDto);
        Task<User> UpdateImage(int Id, IFormFile Image);
        Task<UserImage> GetImage(int Id);
    }
}
