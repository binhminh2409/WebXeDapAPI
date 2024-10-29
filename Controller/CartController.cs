using WebXeDapAPI.Common;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using WebXeDapAPI.Helper;
using WebXeDapAPI.Service.Interfaces;
using Data.Dto;
using WebXeDapAPI.Dto;

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
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult CreateCart([FromBody] List<CartDto> cartDtoList)
        {
            try
            {
                foreach (var cartDto in cartDtoList)
                {
                    var create = _cartService.CreateBicycle(cartDto);
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
        public IActionResult DeleteCart([FromQuery]int productId)
        {
            try
            {
                if (productId == null)
                {
                    throw new Exception("Id not found");
                }
                var delete = _cartService.Delete(productId);
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

        [HttpGet("GetCartGuId")]
        //[Authorize]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult GetCartGuId([FromQuery] string GuId)
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
                if (GuId == null)
                {
                    throw new Exception("userId not found");
                }
                var cart = _cartService.GetCartGuId(GuId);
                return Ok(new XBaseResult
                {
                    data = cart,
                    success = true,
                    httpStatusCode = (int)HttpStatusCode.OK,
                    totalCount = cart.Count,
                    message = "List"
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

        [HttpPut("IncreaseQuantityShoppingCartGuiId")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult IncreaseQuantityShoppingCartGuiId([FromQuery] string guiId, int createProductId)
        {
            try
            {
                if (guiId == null && createProductId == null)
                {
                    throw new Exception("UserId && createProductId not found");
                }
                var increase = _cartService.IncreaseQuantityShoppingCartGuiId(guiId, createProductId);
                return Ok(new XBaseResult
                {
                    data = increase,
                    success = true,
                    httpStatusCode = (int)HttpStatusCode.OK,
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
        [HttpPut("ReduceShoppingCartGuiId")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult ReduceShoppingCartGuiId([FromQuery] string guiId, int createProductId)
        {
            try
            {
                if (guiId == null && createProductId == null)
                {
                    throw new Exception("UserId && createProductId not found");
                }
                var query = _cartService.ReduceShoppingCartGuiId(guiId, createProductId);
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

        [HttpPost("CreateCartslide")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult CreateCartslide([FromBody] List<CartDtoslide> cartDtoListslide)
        {
            try
            {
                foreach (var cartDto in cartDtoListslide)
                {
                    var create = _cartService.CreateBicycleslide(cartDto);
                }

                return Ok(new XBaseResult
                {
                    success = true,
                    httpStatusCode = (int)HttpStatusCode.OK,
                    totalCount = cartDtoListslide.Count,
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
    }
}
