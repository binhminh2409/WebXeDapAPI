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
using WebXeDapAPI.Repository.Interface;

namespace WebXeDapAPI.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeliveryController : ControllerBase
    {
        private readonly IDeliveryIService _deliveryService;

        private readonly IStockIService _stockService;

        private readonly IOrderIService _orderService;

        private readonly Token _token;

        public DeliveryController(IDeliveryIService deliveryService,
        Token token,
        IStockIService stockService,
        IOrderIService orderService)
        {
            _deliveryService = deliveryService;
            _token = token;
            _stockService = stockService;
            _orderService = orderService;
        }


        [HttpPost("Create")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<DeliveryDto>> Create([FromBody] PaymentDto paymentDto)
        {
            try
            {
                var deliveryDto = await _deliveryService.CreateSelfAsync(paymentDto);

                return Ok(new XBaseResult
                {
                    data = deliveryDto,
                    success = true,
                    httpStatusCode = (int)HttpStatusCode.OK,
                    message = "Delivery created"
                });

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

        [HttpGet("All")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<DeliveryDto>> GetAll()
        {
            try
            {
                var deliveryDtos = await _deliveryService.FindAll();

                return Ok(new XBaseResult
                {
                    data = deliveryDtos,
                    success = true,
                    httpStatusCode = (int)HttpStatusCode.OK,
                    message = "Delivery created"
                });

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

        [HttpGet("{deliveryId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<DeliveryDto>> GetById(int deliveryId)
        {
            try
            {
                var deliveryDto = await _deliveryService.FindById(deliveryId);

                return Ok(new XBaseResult
                {
                    data = deliveryDto,
                    success = true,
                    httpStatusCode = (int)HttpStatusCode.OK,
                    message = "Delivery retreived"
                });

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

        [HttpGet("Order/{orderId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<DeliveryDto>> GetByOrdertId(int orderId)
        {
            try
            {
                var deliveryDto = await _deliveryService.FindByOrderId(orderId);

                return Ok(new XBaseResult
                {
                    data = deliveryDto,
                    success = true,
                    httpStatusCode = (int)HttpStatusCode.OK,
                    message = "Delivery retreived"
                });

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

        [HttpPut("Update")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<DeliveryDto>> Update([FromBody] DeliveryDto deliveryDto)
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

                    var updatedDto = await _deliveryService.UpdateAsync(deliveryDto);
                    if (updatedDto.Status == "Completed")
                    {
                        OrderWithDetailDto orderWithDetail = _orderService.GetByIdWithDetail(updatedDto.Payment.OrderId);
                        await _stockService.DecreaseQuantityByOrderWithDetail(orderWithDetail);
                    }

                    return Ok(new XBaseResult
                    {
                        data = updatedDto,
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


        [HttpGet("GetStatus")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<DeliveryDto>> GetStatus()
        {
            try
            {
                var statusList = Enum.GetNames(typeof(StatusDelivery)).ToList();


                return Ok(new XBaseResult
                {
                    data = statusList,
                    success = true,
                    httpStatusCode = (int)HttpStatusCode.OK,
                    message = "Delivery retreived"
                });
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

