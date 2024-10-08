using WebXeDapAPI.Common;
using WebXeDapAPI.Dto;
using WebXeDapAPI.Models;
using WebXeDapAPI.Models.Enum;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;
using WebXeDapAPI.Helper;
using WebXeDapAPI.Service.Interfaces;
using Data.Dto;

namespace WebXeDapAPI.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartIService _cartService;
        private readonly Token _token;
        public CartController(ICartIService cartService,Token token)
        {
            _token = token;
            _cartService = cartService;
        }
        [HttpPost("CreateCart")]
        [Authorize]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult CreateCart([FromBody] List<CartDto> cartDtoList)
        {
            try
            {
                var userClaims = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
                if (userClaims != null && int.TryParse(userClaims.Value, out int userID))
                {
                    var tokenStatus = _token.CheckTokenStatus(userID);
                    if (tokenStatus == StatusToken.Expired)
                    {
                        return Unauthorized("The token is no longer valid. Please log in again.");
                    }
                }
                foreach (var cartDto in cartDtoList)
                {
                    var create = _cartService.CrateBicycle(cartDto);
                   
                }
                return Ok(new XBaseResult
                {
                    success = true,
                    httpStatusCode = (int)HttpStatusCode.OK,
                    totalCount = cartDtoList.Count,
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
        [HttpPut("IncreaseQuantityShoppingCart")]
        //[Authorize]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult IncreaseQuantityShoppingCart([FromQuery]int UserId,int createProductId)
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
                if(UserId == null && createProductId == null)
                {
                    throw new Exception("UserId && createProductId not found");
                }
                var increase = _cartService.IncreaseQuantityShoppingCart(UserId, createProductId);
                return Ok(new XBaseResult
                {
                    data = increase,
                    success = true,
                    httpStatusCode = (int)HttpStatusCode.OK,
                });
            }
            catch(Exception ex)
            {
                return BadRequest(new XBaseResult
                {
                    success = false,
                    httpStatusCode = (int)HttpStatusCode.BadRequest,
                    message = ex.Message
                });
            }
        }
        [HttpPut("ReduceShoppingCart")]
        //[Authorize]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult ReduceShoppingCart([FromQuery]int UserId,int createProductId)
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
                if (UserId == null && createProductId == null)
                {
                    throw new Exception("UserId && createProductId not found");
                }
                var query = _cartService.ReduceShoppingCart(UserId, createProductId);
                return Ok(new XBaseResult
                {
                    data = query,
                    success = true,
                    httpStatusCode = (int)HttpStatusCode.OK,
                    message = "ReduceShoppingCart Successfully"
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
        [HttpGet("GetCart")]
        //[Authorize]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult GetCart([FromQuery]int userId)
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
                if(userId == null)
                {
                    throw new Exception("userId not found");
                }
                var cart = _cartService.GetCart(userId);
                return Ok(new XBaseResult
                {
                    data = cart,
                    success = true,
                    httpStatusCode = (int)HttpStatusCode.OK,
                    totalCount = cart.Count,
                    message = "List"
                });
            }
            catch(Exception ex)
            {
                return BadRequest(new XBaseResult
                {
                    success = false,
                    httpStatusCode = (int)HttpStatusCode.BadRequest,
                    message = ex.Message
                });
            }
        }
        [HttpDelete("Delete")]
        //[Authorize]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult DeleteCart([FromQuery]int Id)
        {
            try
            {
                if (Id == null)
                {
                    throw new Exception("Id not found");
                }
                var delete = _cartService.Delete(Id);
                return Ok(new XBaseResult
                {
                    data = delete,
                    success = true,
                    httpStatusCode = (int)HttpStatusCode.OK,
                    message = "Delete Successfully"
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
        [HttpDelete("DeleteCartId")]
        //[Authorize]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult DeleteCartId([FromQuery]int userid, List<int> productIds)
        {
            try
            {
                if(userid == null)
                {
                    throw new Exception("userid not found");
                }
                var deleteCartId = _cartService.DeleteCart(userid,productIds);
                return Ok(new XBaseResult
                {
                    data = deleteCartId,
                    success = true,
                    httpStatusCode = (int)HttpStatusCode.OK,
                    message = "DeleteCart Successfully"
                });
            }
            catch(Exception ex)
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
