using WebXeDapAPI.Data;
using WebXeDapAPI.Dto;
using WebXeDapAPI.Models;
using WebXeDapAPI.Models.Enum;
using WebXeDapAPI.Dto;
using WebXeDapAPI.Helper;
using WebXeDapAPI.Repository.Interface;
using WebXeDapAPI.Service.Interfaces;

namespace WebXeDapAPI.Service
{
    public class UserService : IUserIService
    {
        private readonly ApplicationDbContext _DbContex;
        private readonly Token _token;
        private readonly IUserInterface _userInterface;
        public UserService(ApplicationDbContext dbContex, Token token, IUserInterface userInterface)
        {
            _userInterface = userInterface;
            _token = token;
            _DbContex = dbContex;
        }
        public User Login(RequestDto requetDto)
        {
            try
            {
                var user = _DbContex.Users.FirstOrDefault(x => x.Email == requetDto.Email);

                if (user == null)
                {
                    throw new Exception("Email not found!");
                }

                if (!BCrypt.Net.BCrypt.Verify(requetDto.Password, user.Password))
                {
                    throw new Exception("Incorrect password!");
                }

                UpdateOrCreateAccessToken(user);

                return user;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while logging in.", ex);
            }
        }

        public bool logout(int UserId)
        {
            try
            {
                var userToken = _userInterface.GetValidTokenByUserId(UserId);
                if(userToken != null)
                {
                    var tokenValue = userToken.AcessToken;
                    var principal = _token.ValidataToken(tokenValue);
                    if (principal != null)
                    {
                        userToken.Status = StatusToken.Expired;
                    }
                }
                _DbContex.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during logout for user {UserId}: {ex.Message}");
                return false;
            }
        }

        public UserDto RegisterUser(UserDto userdto)
        {
            try
            {
                if (userdto == null)
                {
                    throw new ArgumentNullException(nameof(userdto), "User object is null or missing required information.");
                }
                var user = new User
                {
                    Name = userdto.Name,
                    Email = userdto.Email,
                    Password = BCrypt.Net.BCrypt.HashPassword(userdto.Password),
                    Address = userdto.Address,
                    City = userdto.City,
                    Phone = userdto.Phone,
                    DateOfBirth = userdto.DateOfBirth,
                };
                user.roles = Roles.User;
                user.Create = DateTime.Now;
                _DbContex.Users.Add(user);
                _DbContex.SaveChanges();
                return userdto;
            }
            catch (Exception ex)
            {
                throw new Exception("There is an error when creating a User", ex);
            }
        }

        public UserDto RegisterUserAdmin(UserDto userdto)
        {
            try
            {
                if (userdto == null)
                {
                    throw new ArgumentNullException(nameof(userdto), "User object is null or missing required information.");
                }
                var user = new User
                {
                    Name = userdto.Name,
                    Email = userdto.Email,
                    Password = BCrypt.Net.BCrypt.HashPassword(userdto.Password),
                    Address = userdto.Address,
                    City = userdto.City,
                    Phone = userdto.Phone,
                    DateOfBirth = userdto.DateOfBirth,
                };
                user.roles = Roles.ManagerMent;
                user.Create = DateTime.Now;
                _DbContex.Users.Add(user);
                _DbContex.SaveChanges();
                return userdto;
            }
            catch (Exception ex)
            {
                throw new Exception("There is an error when creating a User", ex);
            }
        }

        public string ResetPassword(int UserId)
        {
            throw new NotImplementedException();
        }
        public void UpdateOrCreateAccessToken(User user)
        {
            var existingToken = _userInterface.GetValidTokenByUserId(user.Id);
            if(existingToken != null)
            {
                var token = _token.CreateToken(user);
                if (string.IsNullOrEmpty(token))
                    throw new Exception("Failed to create a token.");
                existingToken.AcessToken = token;
                existingToken.ExpirationDate = DateTime.Now;
            }
            else
            {
                var token = _token.CreateToken(user);
                if (string.IsNullOrEmpty(token))
                    throw new Exception("Failed to create a token.");
                var accessToken = new AccessToken
                {
                    UserId = user.Id,
                    AcessToken = token,
                    Status = StatusToken.Valid,
                    ExpirationDate = DateTime.Now,
                };
                _DbContex.AccessTokens.Add(accessToken);
            }
            _DbContex.SaveChanges();
        }
    }
}
