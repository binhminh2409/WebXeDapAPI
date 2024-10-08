using WebXeDapAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebXeDapAPI.Helper;

namespace WebXeDapAPI.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class DecodeTokenController : ControllerBase
    {
        private readonly Token _token;
        public DecodeTokenController(Token token)
        {
            _token = token;
        }
        [Authorize]
        [HttpGet("extract-user-id")]
        public IActionResult ExtractUserIdFromToken([FromQuery] string token)
        {
            try
            {
                int? userId = _token.ExtractUserIdFromToken(token);
                if (userId.HasValue)
                {
                    return Ok($"User ID extracted from token: {userId}");
                }
                else
                {
                    return BadRequest("Failed to extract user ID from token.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
    }
}
