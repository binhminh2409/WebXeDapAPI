using WebXeDapAPI.Dto;

namespace WebXeDapAPI.Service.Interfaces
{
    public interface IAdminService
    {
        UserDto CreateEmployee(UserDto userDto); // Tạo tài khoản nhân viên
        IEnumerable<UserDto> GetEmployees(); // Lấy danh sách nhân viên
        bool DeleteEmployee(int employeeId); // Xóa nhân viên
    }
}
