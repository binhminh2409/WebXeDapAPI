using Microsoft.AspNetCore.Mvc;
using WebXeDapAPI.Dto;
using WebXeDapAPI.Service.Interfaces;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using WebXeDapAPI.Helper;
using WebXeDapAPI.Models;
using WebXeDapAPI.Common;
using System.Security.Claims;
using WebXeDapAPI.Models.Enum;

namespace WebXeDapAPI.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeliveryController : ControllerBase
    {
        private readonly IDeliveryIService _deliveryService;
        private readonly Token _token;

        public DeliveryController(IDeliveryIService deliveryService, Token token)
        {
            _deliveryService = deliveryService;
            _token = token;
        }


        [HttpPost("Create")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<DeliveryDto>> Create([FromBody] PaymentDto paymentDto, string cityFrom, string cityTo, string districtFrom, string districtTo)
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

                    var deliveryDto = await _deliveryService.CreateAsync(paymentDto, cityFrom, cityTo, districtFrom, districtTo);

                    return Ok(new XBaseResult
                    {
                        data = deliveryDto,
                        success = true,
                        httpStatusCode = (int)HttpStatusCode.OK,
                        message = "Delivery created"
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
                    message = "An error occurred while creating delivery: " + ex.Message
                });
            }
        }

        [HttpPost("All")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<DeliveryDto>> GetAll()
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

                    var deliveryDtos = await _deliveryService.FindAll();

                    return Ok(new XBaseResult
                    {
                        data = deliveryDtos,
                        success = true,
                        httpStatusCode = (int)HttpStatusCode.OK,
                        message = "Delivery created"
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
                    message = "An error occurred while creating delivery: " + ex.Message
                });
            }
        }

        [HttpGet("Delivery/{deliveryId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<DeliveryDto>> GetById(int deliveryId)
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

                    var deliveryDto = await _deliveryService.FindById(deliveryId);

                    return Ok(new XBaseResult
                    {
                        data = deliveryDto,
                        success = true,
                        httpStatusCode = (int)HttpStatusCode.OK,
                        message = "Delivery retreived"
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
                    message = "An error occurred while creating delivery: " + ex.Message
                });
            }
        }

        [HttpGet("Delivery/Order/{orderId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<DeliveryDto>> GetByOrdertId(int orderId)
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

                    var deliveryDto = await _deliveryService.FindByOrderId(orderId);

                    return Ok(new XBaseResult
                    {
                        data = deliveryDto,
                        success = true,
                        httpStatusCode = (int)HttpStatusCode.OK,
                        message = "Delivery retreived"
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
                    message = "An error occurred while creating delivery: " + ex.Message
                });
            }
        }

        [HttpGet("MyDeliveries")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<DeliveryDto>> GetByUser()
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

                    var deliveryDto = await _deliveryService.FindByUser(userId);

                    return Ok(new XBaseResult
                    {
                        data = deliveryDto,
                        success = true,
                        httpStatusCode = (int)HttpStatusCode.OK,
                        message = "Delivery retreived"
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
                    message = "An error occurred while creating delivery: " + ex.Message
                });
            }
        }
    }
}

