using WebXeDapAPI.Common;
using WebXeDapAPI.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using WebXeDapAPI.Service.Interfaces;

namespace WebXeDapAPI.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderIService _orderService;
        public OrderController(IOrderIService orderService)
        {
            _orderService = orderService;
        }
        [HttpPost("CreateOrder")]
        //[Authorize]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult CreateOrder([FromBody]OrderDto orderDto)
        {
            try
            {
                //var userClaims = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
                //if (userClaims != null && int.TryParse(userClaims.Value, out int userID))
                //{
                //    var tokenStatus = _token.CheckTokenStatus(userID);
                //    if (tokenStatus == StatusToken.Expired)
                //    {
                //        return Unauthorized("The token is no longer valid. Please log in again.");
                //    }
                //}
                if(orderDto == null)
                {
                    throw new ArgumentNullException(nameof(orderDto), "Invalid slide data");
                }
                var order = _orderService.Create(orderDto);
                return Ok(new XBaseResult
                {
                    data = orderDto,
                    success = true,
                    httpStatusCode = (int)HttpStatusCode.OK,
                    message = "Create Successfully"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new XBaseResult
                {
                    success = false,
                    httpStatusCode= (int)HttpStatusCode.BadRequest,
                    message = ex.Message
                });
            }
        }
    }
}
