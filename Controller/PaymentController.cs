using WebXeDapAPI.Common;
using WebXeDapAPI.Dto;
using WebXeDapAPI.Models.Enum;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;
using WebXeDapAPI.Helper;
using WebXeDapAPI.Service.Interfaces;
using WebXeDapAPI.Models;


namespace WebXeDapAPI.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentIService _paymentIService;
        private readonly IUserIService _userIService;
        private readonly Token _token;
        private readonly IVietqrService _vietqrService;

        public PaymentController(IPaymentIService paymentIService,
            Token token,
            IUserIService userIService,
            IVietqrService vietqrService)
        {
            _paymentIService = paymentIService;
            _token = token;
            _userIService = userIService;
            _vietqrService = vietqrService;
        }

        [HttpGet("All")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetAllPayments()
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

                    var payments = await _paymentIService.FindAll();

                    return Ok(new XBaseResult
                    {
                        data = payments,
                        success = true,
                        httpStatusCode = (int)HttpStatusCode.OK,
                        message = "Get all payments successfully",
                        totalCount = payments?.Count ?? 0
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
                    message = "An error occurred while getting payments: " + ex.Message
                });
            }
        }

        [HttpGet("MyPayments")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetMyPayments()
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

                    var payments = await _paymentIService.FindByUser(userId);

                    return Ok(new XBaseResult
                    {
                        data = payments,
                        success = true,
                        httpStatusCode = (int)HttpStatusCode.OK,
                        message = "Get all my payments successfully",
                        totalCount = payments?.Count ?? 0
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

                    var payment = await _paymentIService.CreateAsync(paymentDto);

                    return Ok(new XBaseResult
                    {
                        data = payment,
                        success = true,
                        httpStatusCode = (int)HttpStatusCode.OK,
                        totalCount = payment.Id,
                        message = "Payment created successfully"
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

                    var confirmedPayment = await _paymentIService.ConfirmAsync(paymentId);

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

                    var confirmedPayment = await _paymentIService.UpdateStatusAsync(paymentDto);

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
