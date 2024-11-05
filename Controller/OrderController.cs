using WebXeDapAPI.Common;
using WebXeDapAPI.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using WebXeDapAPI.Service.Interfaces;
using WebXeDapAPI.Models;
using WebXeDapAPI.Helper;
using WebXeDapAPI.Models.Enum;
using System.Security.Claims;

namespace WebXeDapAPI.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderIService _orderService;

        private readonly Token _token;

        public OrderController(IOrderIService orderService
                            , Token token)
        {
            _orderService = orderService;
            _token = token;
        }

        [HttpGet("ListOfBestSellingProducts")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult ListOfBestSellingProducts()
        {
            try
            {
                var order = _orderService.ListOfBestSellingProducts();
                return Ok(new XBaseResult
                {
                    data = order,
                    success = true,
                    httpStatusCode = (int)HttpStatusCode.OK,
                    totalCount = order.Id,
                    message = "Create Successfully"
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

        [HttpPost("CreateOrder")]
        //[Authorize]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult CreateOrder([FromBody] OrderDto orderDto)
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
                if (orderDto == null)
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
                    httpStatusCode = (int)HttpStatusCode.BadRequest,
                    message = ex.Message
                });
            }
        }

        [HttpGet("MyOrders")]
        [Authorize(Roles = "User")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult GetByUser()
        {
            try
            {
                var userIdClaim = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
                {
                    var tokenStatus = _token.CheckTokenStatus(userId);
                    if (tokenStatus == StatusToken.Expired)
                    {
                        // Token không còn hợp lệ, từ chối yêu cầu
                        return Unauthorized("The token is no longer valid. Please log in again.");
                    }

                    var orderDtos = _orderService.GetByUser(userId);

                    return Ok(new XBaseResult
                    {
                        data = orderDtos,
                        success = true,
                        httpStatusCode = (int)HttpStatusCode.OK,
                        message = "Get all my orders successfully",
                        totalCount = orderDtos?.Count ?? 0
                    });
                }

                return BadRequest("Invalid user ID.");
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

        [HttpGet("MyOrdersDetails")]
        [Authorize(Roles = "User")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult GetDetailsByUser()
        {
            try
            {
                var userIdClaim = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
                {
                    var tokenStatus = _token.CheckTokenStatus(userId);
                    if (tokenStatus == StatusToken.Expired)
                    {
                        // Token không còn hợp lệ, từ chối yêu cầu
                        return Unauthorized("The token is no longer valid. Please log in again.");
                    }

                    var orderDetails = _orderService.GetDetailsByUser(userId);

                    return Ok(new XBaseResult
                    {
                        data = orderDetails,
                        success = true,
                        httpStatusCode = (int)HttpStatusCode.OK,
                        message = "Get all my order details successfully",
                        totalCount = orderDetails?.Count ?? 0
                    });
                }

                return BadRequest("Invalid user ID.");
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

        [HttpGet("MyOrders/{orderId}")]
        [Authorize(Roles = "User")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult GetByIdWithDetail(int orderId)
        {
            try
            {
                var userIdClaim = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
                {
                    var tokenStatus = _token.CheckTokenStatus(userId);
                    if (tokenStatus == StatusToken.Expired)
                    {
                        // Token is no longer valid; deny request
                        return Unauthorized("The token is no longer valid. Please log in again.");
                    }

                    var orderWithDetailDto = _orderService.GetByIdWithDetail(orderId);

                    return Ok(new XBaseResult
                    {
                        data = orderWithDetailDto,
                        success = true,
                        httpStatusCode = (int)HttpStatusCode.OK,
                        message = "Get all my order details successfully"
                    });
                }

                return BadRequest("Invalid user ID.");
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

        [HttpGet("Reports/{orderId}")]
        [Authorize(Roles = "ManagerMent")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult GetByIdWithDetailAdmin(int orderId)
        {
            try
            {
                var orderWithDetailDto = _orderService.GetByIdWithDetail(orderId);
                return Ok(new XBaseResult
                {
                    data = orderWithDetailDto,
                    success = true,
                    httpStatusCode = (int)HttpStatusCode.OK,
                    message = "Get all my order details successfully"
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

        [HttpPut("Cancel/{orderId}")]
        [Authorize(Roles = "User")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult CancelOrder(int orderId)
        {
            Console.WriteLine("Cancelling order");
            try
            {
                var userIdClaim = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);

                Console.WriteLine(userIdClaim);
                if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
                {
                    var tokenStatus = _token.CheckTokenStatus(userId);
                    if (tokenStatus == StatusToken.Expired)
                    {
                        // Token không còn hợp lệ, từ chối yêu cầu
                        return Unauthorized("The token is no longer valid. Please log in again.");
                    }

                    var message = _orderService.CancelOrder(orderId);

                    return Ok(new XBaseResult
                    {
                        data = message,
                        success = true,
                        httpStatusCode = (int)HttpStatusCode.OK,
                        message = "Get all my orders successfully",
                    });
                }

                return BadRequest("Invalid user ID.");
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
    }
}
