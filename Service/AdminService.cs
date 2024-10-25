using WebXeDapAPI.Dto;
using WebXeDapAPI.Models;
using WebXeDapAPI.Models.Enum;
using WebXeDapAPI.Data;
using WebXeDapAPI.Service.Interfaces;

namespace WebXeDapAPI.Service
{
    public class AdminService : IAdminService
    {
        private readonly ApplicationDbContext _dbContext;

        public AdminService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // Tạo tài khoản nhân viên
        public UserDto CreateEmployee(UserDto userDto)
        {
            if (userDto == null)
            {
                throw new ArgumentNullException(nameof(userDto), "User data is missing.");
            }

            var employee = new User
            {
                Name = userDto.Name,
                Email = userDto.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(userDto.Password),
                Address = userDto.Address,
                City = userDto.City,
                Phone = userDto.Phone,
                DateOfBirth = userDto.DateOfBirth,
                roles = Roles.ManagerMent, // Đặt role là ManagerMent cho nhân viên
                Create = DateTime.Now
            };

            _dbContext.Users.Add(employee);
            _dbContext.SaveChanges();

            return userDto;
        }

        // Lấy danh sách tất cả nhân viên
        public IEnumerable<UserDto> GetEmployees()
        {
            var employees = _dbContext.Users
                .Where(u => u.roles == Roles.ManagerMent)
                .Select(u => new UserDto
                {  
                    Name = u.Name,
                    Email = u.Email,
                    Address = u.Address,
                    City = u.City,
                    Phone = u.Phone,
                    DateOfBirth = u.DateOfBirth
                }).ToList();

            return employees;
        }

        public bool DeleteEmployeeByEmail(string email)
        {
            var user = _dbContext.Users.FirstOrDefault(u => u.Email == email && u.roles == Roles.ManagerMent);
            if (user != null)
            {
                _dbContext.Users.Remove(user);
                _dbContext.SaveChanges();
                return true;
            }
            return false;
        }
    }
}
