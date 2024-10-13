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

        public PaymentController(IPaymentIService paymentIService, Token token, IUserIService userIService){
            _paymentIService = paymentIService;
            _token = token;
            _userIService = userIService;
        }

        [HttpPost("Create")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreatePayment([FromForm] PaymentDto paymentDto)
        {
            try
            {
                if (paymentDto == null)
                {
                    return BadRequest(new XBaseResult
                    {
                        success = false,
                        httpStatusCode = (int)HttpStatusCode.BadRequest,
                        message = "Invalid product data"
                    });
                }
                var userIdClaim = HttpContext.User.FindFirst("Id");


                if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
                {
                    var tokenStatus = _token.CheckTokenStatus(userId);

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
    }
}
