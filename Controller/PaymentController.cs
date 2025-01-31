using WebXeDapAPI.Common;
using WebXeDapAPI.Dto;
using WebXeDapAPI.Models.Enum;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;
using WebXeDapAPI.Helper;
using WebXeDapAPI.Service.Interfaces;
using WebXeDapAPI.Models;
using WebXeDapAPI.Repository.Interface;
using WebXeDapAPI.Service;
using Microsoft.AspNetCore.Authorization;


namespace WebXeDapAPI.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentIService _paymentService;
        private readonly IUserIService _userIService;
        private readonly Token _token;
        private readonly IVietqrService _vietqrService;

        private readonly IOrderIService _orderService;

        private readonly IStockIService _stockService;


        public PaymentController(IPaymentIService paymentIService,
            Token token,
            IUserIService userIService,
            IVietqrService vietqrService,
            IOrderIService orderIService,
            IStockIService stockService)
        {
            _paymentService = paymentIService;
            _token = token;
            _userIService = userIService;
            _vietqrService = vietqrService;
            _orderService = orderIService;
            _stockService = stockService;
        }

        [HttpGet("All")]
        // [Authorize(Roles = "ManageMent")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetAllPayments()
        {
            try
            {
                var payments = await _paymentService.FindAll();
                return Ok(new XBaseResult
                {
                    data = payments,
                    success = true,
                    httpStatusCode = (int)HttpStatusCode.OK,
                    message = "Get all payments successfully",
                    totalCount = payments?.Count ?? 0
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new XBaseResult
                {
                    success = false,
                    httpStatusCode = (int)HttpStatusCode.BadRequest,
                    message = "An error occurred while getting payments: " + ex.Message
                });
            }
        }

        [HttpGet("MyPayments")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetMyPayments([FromQuery] string? myGuid)
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

                    var payments = await _paymentService.FindByUser(userId);

                    return Ok(new XBaseResult
                    {
                        data = payments,
                        success = true,
                        httpStatusCode = (int)HttpStatusCode.OK,
                        message = "Get all my payments successfully",
                        totalCount = payments?.Count ?? 0
                    });
                }
                else
                {
                    var payments = await _paymentService.FindByGuid(myGuid);

                    return Ok(new XBaseResult
                    {
                        data = payments,
                        success = true,
                        httpStatusCode = (int)HttpStatusCode.OK,
                        message = "Get all my payments successfully",
                        totalCount = payments?.Count ?? 0
                    });
                }

            }
            catch (Exception ex)
            {
                return BadRequest(new XBaseResult
                {
                    success = false,
                    httpStatusCode = (int)HttpStatusCode.BadRequest,
                    message = "An error occurred while getting payments: " + ex.Message
                });
            }
        }

        [HttpPost("Create")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreatePayment([FromBody] PaymentDto paymentDto)
        {
            try
            {
                if (paymentDto == null)
                {
                    return BadRequest(new XBaseResult
                    {
                        success = false,
                        httpStatusCode = (int)HttpStatusCode.BadRequest,
                        message = "Invalid payment id"
                    });
                }
                var userIdClaim = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);

                if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
                {
                    var tokenStatus = _token.CheckTokenStatus(userId);
                    paymentDto.UserId = userId;
                    if (tokenStatus == StatusToken.Expired)
                    {
                        // Token không còn hợp lệ, từ chối yêu cầu
                        return Unauthorized("The token is no longer valid. Please log in again.");
                    }

                    var payment = await _paymentService.CreateAsync(paymentDto);

                    return Ok(new XBaseResult
                    {
                        data = payment,
                        success = true,
                        httpStatusCode = (int)HttpStatusCode.OK,
                        totalCount = payment.Id,
                        message = "Payment created successfully"
                    });
                }

                var nonLoggedInPayment = await _paymentService.CreateAsync(paymentDto);

                return Ok(new XBaseResult
                {
                    data = nonLoggedInPayment,
                    success = true,
                    httpStatusCode = (int)HttpStatusCode.OK,
                    message = "Get all my payments successfully",
                });


            }
            catch (Exception ex)
            {
                return BadRequest(new XBaseResult
                {
                    success = false,
                    httpStatusCode = (int)HttpStatusCode.BadRequest,
                    message = "An error occurred while creating payment: " + ex.Message
                });
            }
        }

        [HttpPut("Confirm")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> ConfirmPayment([FromQuery] int paymentId)
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

                    var confirmedPayment = await _paymentService.ConfirmAsync(paymentId);

                    return Ok(new XBaseResult
                    {
                        data = confirmedPayment,
                        success = true,
                        httpStatusCode = (int)HttpStatusCode.OK,
                        totalCount = confirmedPayment.Id,
                        message = "Payment confirmed"
                    });
                }
                var nonLoggedInPayment = await _paymentService.ConfirmAsync(paymentId);

                return Ok(new XBaseResult
                {
                    data = nonLoggedInPayment,
                    success = true,
                    httpStatusCode = (int)HttpStatusCode.OK,
                    message = "Get all my payments successfully",
                });

            }
            catch (Exception ex)
            {
                return BadRequest(new XBaseResult
                {
                    success = false,
                    httpStatusCode = (int)HttpStatusCode.BadRequest,
                    message = "An error occurred while confirming payment: " + ex.Message
                });
            }
        }


        [HttpPut("UpdateStatus")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> UpdateStatusPayment([FromQuery] PaymentDto paymentDto)
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

                    var confirmedPayment = await _paymentService.UpdateStatusAsync(paymentDto);

                    return Ok(new XBaseResult
                    {
                        data = confirmedPayment,
                        success = true,
                        httpStatusCode = (int)HttpStatusCode.OK,
                        totalCount = confirmedPayment.Id,
                        message = "Payment confirmed"
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
                    message = "An error occurred while confirming payment: " + ex.Message
                });
            }
        }

        [HttpGet("generate-qr")]
        public async Task<IActionResult> GenerateQrCode(string bank, string accountNumber, string amount, string ndck, string fullName)
        {
            try
            {
                var qrCodeBytes = await _vietqrService.GenerateQrCodeAsync(bank, accountNumber, amount, ndck, fullName);
                return File(qrCodeBytes, "image/png");
            }
            catch (HttpRequestException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
