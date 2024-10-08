using WebXeDapAPI.Common;
using WebXeDapAPI.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using WebXeDapAPI.Repository.Interface;
using WebXeDapAPI.Service.Interfaces;

namespace WebXeDapAPI.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class Product_DetailsController : ControllerBase
    {
        private readonly IProduct_DetailIService _product_DetailIService;
        private readonly IProducts_DrtailInterface _products_DrtailInterface;
        public Product_DetailsController(IProduct_DetailIService product_DetailIService,IProducts_DrtailInterface products_DrtailInterface)
        {
            _products_DrtailInterface = products_DrtailInterface;
            _product_DetailIService = product_DetailIService;
        }
        [HttpPost("Create")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult CreateProduct_Details([FromForm]Product_DetailDto product_DetailDto)
        {
            try
            {
                if(product_DetailDto == null)
                {
                    return BadRequest("Invalid slide data");
                }
                var createProducts = _product_DetailIService.Create(product_DetailDto);
                return Ok(new XBaseResult
                {
                    data = createProducts,
                    success = true,
                    httpStatusCode = (int)HttpStatusCode.OK,
                    totalCount = createProducts.Id,
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
        [HttpPut("Update")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult UpdateProduct_Details([FromBody]UpdateProduct_DetailsDto updateproduct_DetailDto)
        {
            try
            {
                if(updateproduct_DetailDto == null)
                {
                    return BadRequest("Invalid slide data");
                }
                var update = _product_DetailIService.Update(updateproduct_DetailDto);
                return Ok(new XBaseResult
                {
                    data = update,
                    success = true,
                    httpStatusCode = (int)HttpStatusCode.OK,
                    totalCount = update.Count(),
                    message = "Update Successfully"
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
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult DeleteProduct_Detail(int Id)
        {
            try
            {
                if(Id == null)
                {
                    return BadRequest("Id not found");
                }
                var delete = _product_DetailIService.Delete(Id);
                return Ok(new XBaseResult
                {
                    data = delete,
                    success = true,
                    httpStatusCode = (int)HttpStatusCode.OK,
                    message = "Delete Successfully"
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
        [HttpGet("GetProducts_Detail")]
        //[Authorize]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult GetProduct_Detail([FromQuery]int productId)
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
                if (productId <= 0)
                {
                    return BadRequest("Id not found");
                }
                var query = _products_DrtailInterface.Getproducts_Detail(productId);
                return Ok(new XBaseResult
                {
                    data = query,
                    success = true,
                    httpStatusCode = (int)HttpStatusCode.OK,
                    message = "Successfully"
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
