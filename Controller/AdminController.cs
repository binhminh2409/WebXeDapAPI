using Microsoft.AspNetCore.Mvc;
using WebXeDapAPI.Dto;
using WebXeDapAPI.Service.Interfaces;
using System.Net;
using Microsoft.AspNetCore.Authorization;

namespace WebXeDapAPI.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "ManagerMent")]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;

        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        // API để tạo tài khoản nhân viên
        [HttpPost("CreateEmployee")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult CreateEmployee(UserDto userDto)
        {
            try
            {
                var employee = _adminService.CreateEmployee(userDto);
                return Ok(new
                {
                    success = true,
                    httpStatusCode = (int)HttpStatusCode.OK,
                    data = employee,
                    message = "Employee created successfully."
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    success = false,
                    httpStatusCode = (int)HttpStatusCode.BadRequest,
                    message = ex.Message
                });
            }
        }

        // API để lấy danh sách nhân viên
        [HttpGet("GetEmployees")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult GetEmployees()
        {
            try
            {
                var employees = _adminService.GetEmployees();
                return Ok(new
                {
                    success = true,
                    httpStatusCode = (int)HttpStatusCode.OK,
                    data = employees
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    success = false,
                    httpStatusCode = (int)HttpStatusCode.BadRequest,
                    message = ex.Message
                });
            }
        }

        // API để xóa nhân viên
        [HttpDelete("DeleteEmployee/{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult DeleteEmployee(int id)
        {
            try
            {
                var result = _adminService.DeleteEmployee(id);
                if (result)
                {
                    return Ok(new
                    {
                        success = true,
                        httpStatusCode = (int)HttpStatusCode.OK,
                        message = "Employee deleted successfully."
                    });
                }
                return NotFound(new
                {
                    success = false,
                    httpStatusCode = (int)HttpStatusCode.BadRequest,
                    message = "Employee not found."
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    success = false,
                    httpStatusCode = (int)HttpStatusCode.BadRequest,
                    message = ex.Message
                });
            }
        }
    }
}
