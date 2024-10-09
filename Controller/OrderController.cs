using WebXeDapAPI.Common;
using WebXeDapAPI.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using WebXeDapAPI.Service.Interfaces;

// TESTTTTTTTTTTTTTTTTTTTTTTTTTTTT444444
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
        [HttpGet("GetRevenueByType")]
        public IActionResult GetRevenueByType(int typeId, DateTime startDate, DateTime endDate)
        {
            try
            {
                decimal revenue = _orderService.CalculateRevenueByType(typeId, startDate, endDate);
                return Ok(new { Revenue = revenue });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpPost("GetRevenue")]  // Đường dẫn API để lấy doanh thu
        [ProducesResponseType(200)]  // Mã trả về khi thành công
        [ProducesResponseType(400)]  // Mã trả về khi lỗi
        public IActionResult GetRevenue([FromBody] RevenueRequestDto revenueRequestDto)
        {
            try
            {
                if (revenueRequestDto.StartDate == default || revenueRequestDto.EndDate == default)
                {
                    throw new ArgumentException("Khoảng thời gian không hợp lệ được cung cấp");
                }

                var totalRevenue = _orderService.CalculateRevenue(revenueRequestDto.StartDate, revenueRequestDto.EndDate);  // Tính doanh thu trong khoảng thời gian

                return Ok(new XBaseResult
                {
                    data = totalRevenue,  // Dữ liệu là tổng doanh thu
                    success = true,  // Đánh dấu thành công
                    httpStatusCode = (int)HttpStatusCode.OK,  // Mã HTTP thành công
                    message = "Tính doanh thu thành công"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new XBaseResult
                {
                    success = false,  // Đánh dấu thất bại
                    httpStatusCode = (int)HttpStatusCode.BadRequest,  // Mã HTTP lỗi
                    message = ex.Message  // Thông báo lỗi
                });
            }
        }

    }
}
