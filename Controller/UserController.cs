using WebXeDapAPI.Common;
using WebXeDapAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using WebXeDapAPI.Service.Interfaces;
using WebXeDapAPI.Dto;
using WebXeDapAPI.Helper;
using WebXeDapAPI.Service;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using WebXeDapAPI.Models.Enum;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace WebXeDapAPI.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserIService _userIService;
        private readonly Token _token;
        public UserController(IUserIService userIService, Token token)
        {
            _userIService = userIService;
            _token = token;
        }
        [HttpPost("Create")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult CreateLogin(UserDto userdto)
        {
            try
            {
                var create = _userIService.RegisterUser(userdto);
                return Ok(new XBaseResult
                {
                    data = create,
                    success = true,
                    httpStatusCode = (int)HttpStatusCode.OK,
                    message = "User created successfully"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new XBaseResult
                {
                    success = false,
                    httpStatusCode = (int)HttpStatusCode.BadRequest,
                    message = ex.Message
                });
            }
        }

        [HttpPost("Login")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult LoginUser([FromBody] RequestDto requestDto)
        {
            try
            {
                if (requestDto == null || string.IsNullOrEmpty(requestDto.Email) || string.IsNullOrEmpty(requestDto.Password))
                {
                    return BadRequest("Email and password are required.");
                }

                User user = _userIService.Login(requestDto);

                string jwtToken = _token.CreateToken(user);

                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    Expires = DateTime.UtcNow.AddMinutes(3000),
                };

                HttpContext.Response.Cookies.Append("authenticationToken", jwtToken, cookieOptions);

                // Chỉ trả về mã token
                return Ok(jwtToken);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }


        [HttpPost("CreateAdmin")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult CreateLoginAdmin(UserDto userdto)
        {
            try
            {
                var create = _userIService.RegisterUserAdmin(userdto);
                return Ok(new XBaseResult
                {
                    data = create,
                    success = true,
                    httpStatusCode = (int)HttpStatusCode.OK,
                    message = "User successfully"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new XBaseResult
                {
                    success = false,
                    httpStatusCode = (int)HttpStatusCode.BadRequest,
                    message = ex.Message
                });
            }
        }

        [HttpPost("logout")]
        [Authorize]
        public IActionResult Logout()
        {
            try
            {
                var userIdClaim = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
                {
                    // Gọi hàm kiểm tra token
                    var tokenStatus = _token.CheckTokenStatus(userId);

                    if (tokenStatus == StatusToken.Expired)
                    {
                        return Unauthorized("The token is no longer valid. Please log in again.");
                    }

                    // Gọi service logout
                    var result = _userIService.logout(userId);
                    if (result)
                    {
                        HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                        return Ok("Logged out successfully.");
                    }
                    else
                    {
                        return StatusCode(500, "An error occurred during logout.");
                    }
                }
                else
                {
                    return BadRequest("Invalid user ID.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"An error occurred: {ex.Message}");
            }
        }

        [HttpGet("GetViewUser")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetUser(int userId)
        {
            try
            {
                var result = await _userIService.GetUser(userId);
                return Ok(new XBaseResult
                {
                    data = result,
                    success = true,
                    httpStatusCode = (int)HttpStatusCode.OK,
                    message = "User successfully retrieved"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new XBaseResult
                {
                    success = false,
                    httpStatusCode = (int)HttpStatusCode.BadRequest,
                    message = ex.Message
                });
            }
        }

        [HttpPut("UpdateViewUser")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> UpdateViewUser(int userId, [FromBody] UpdateGetViewUser updateGetViewUserDto)
        {
            try
            {
                var result = await _userIService.UpdateViewUser(userId, updateGetViewUserDto);

                return Ok(new XBaseResult
                {
                    data = result,
                    success = true,
                    httpStatusCode = (int)HttpStatusCode.OK,
                    message = "User updated successfully"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new XBaseResult
                {
                    success = false,
                    httpStatusCode = (int)HttpStatusCode.BadRequest,
                    message = ex.Message
                });
            }
        }

        [HttpPut("UpdateImage")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> UpdateImage(int userId, IFormFile image)
        {
            try
            {
                var result = await _userIService.UpdateImage(userId, image);

                return Ok(new XBaseResult
                {
                    data = result,
                    success = true,
                    httpStatusCode = (int)HttpStatusCode.OK,
                    message = "User updated successfully"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new XBaseResult
                {
                    success = false,
                    httpStatusCode = (int)HttpStatusCode.BadRequest,
                    message = ex.Message
                });
            }
        }

        [HttpGet("GetImage")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetUserImage(int userId)
        {
            try
            {
                var result = await _userIService.GetImage(userId);

                return Ok(new XBaseResult
                {
                    data = result,
                    success = true,
                    httpStatusCode = (int)HttpStatusCode.OK,
                    message = "Get User successfully"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new XBaseResult
                {
                    success = false,
                    httpStatusCode = (int)HttpStatusCode.BadRequest,
                    message = ex.Message
                });
            }
        }

        //[HttpGet("GetImage")]
        //[ProducesResponseType(200)]
        //[ProducesResponseType(400)]
        //public async Task<IActionResult> GetUserImage(int userId)
        //{
        //    try
        //    {
        //        var result = await _userIService.GetImage(userId);

        //        return Ok(new XBaseResult
        //        {
        //            data = result,
        //            success = true,
        //            httpStatusCode = (int)HttpStatusCode.OK,
        //            message = "Get User successfully"
        //        });
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(new XBaseResult
        //        {
        //            success = false,
        //            httpStatusCode = (int)HttpStatusCode.BadRequest,
        //            message = ex.Message
        //        });
        //    }
        //}
    }
}