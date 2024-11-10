using WebXeDapAPI.Dto;
using Microsoft.AspNetCore.Mvc;
using WebXeDapAPI.Service.Interfaces;
using WebXeDapAPI.Common;
using System.Net;
using WebXeDapAPI.Repository.Interface;
using WebXeDapAPI.Helper;
using WebXeDapAPI.Service;
using Microsoft.AspNetCore.Http;
using Data.Dto;

namespace WebXeDapAPI.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductsIService _productsService;
        private readonly IProductsInterface _productsInterface;
        private readonly Token _token;
        public ProductsController(IProductsIService productsIService, IProductsInterface productsInterface, Token token)
        {
            _productsInterface = productsInterface;
            _productsService = productsIService;
            _token = token;
        }
        [HttpPost("Create")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateProducts([FromForm] ProductDto productsDto)
        {
            try
            {
                if (productsDto == null)
                {
                    return BadRequest(new XBaseResult
                    {
                        success = false,
                        httpStatusCode = (int)HttpStatusCode.BadRequest,
                        message = "Invalid product data"
                    });
                }
                // Tạo sản phẩm
                var product = await _productsService.Create(productsDto);

                return Ok(new XBaseResult
                {
                    data = product,
                    success = true,
                    httpStatusCode = (int)HttpStatusCode.OK,
                    totalCount = product.Id,
                    message = "Product created successfully"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new XBaseResult
                {
                    success = false,
                    httpStatusCode = (int)HttpStatusCode.BadRequest,
                    message = "An error occurred while creating the product: " + ex.Message
                });
            }
        }

        [HttpPut("Update")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> UpdateProduct(int Id, [FromForm] UpdateProductDto updateproductDto)
        {
            try
            {
                if (updateproductDto == null)
                {
                    return Unauthorized("Invalid slide data");
                }
                var delete = await _productsService.Update(Id, updateproductDto);
                return Ok(new XBaseResult
                {
                    data = updateproductDto,
                    success = true,
                    httpStatusCode = (int)HttpStatusCode.OK,
                    message = "Update Successfully"
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

        [HttpGet("GetBrandName")]
        //[Authorize]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult GetListBrandName([FromQuery] string keyword)
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
                if (keyword == null)
                {
                    return BadRequest(new XBaseResult
                    {
                        success = false,
                        httpStatusCode = (int)HttpStatusCode.BadRequest,
                        message = "Invalid slide data"
                    });
                }

                var query = _productsInterface.GetAllBrandName(keyword);
                return Ok(new XBaseResult
                {
                    data = query,
                    success = true,
                    httpStatusCode = (int)HttpStatusCode.OK,
                    totalCount = query.Count,
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
        [HttpGet("GetTypeName")]
        //[Authorize]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult GetListTypeName([FromQuery] string keyword, int limit = 8)
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
                if (keyword == null)
                {
                    return BadRequest(new XBaseResult
                    {
                        success = false,
                        httpStatusCode = (int)HttpStatusCode.BadRequest,
                        message = "Invalid slide data"
                    });
                }
                var typeName = _productsInterface.GetAllTypeName(keyword, limit);
                return Ok(new XBaseResult
                {
                    data = typeName,
                    success = true,
                    httpStatusCode = (int)HttpStatusCode.OK,
                    totalCount = typeName.Count,
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
        [HttpGet("GetPriceHasDecreased")]
        //[Authorize]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult GetListPriceHasDecreased()
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
                var PriceHasDecreased = _productsInterface.SearchProductsByPriceHasDecreased();
                return Ok(new XBaseResult
                {
                    data = PriceHasDecreased,
                    success = true,
                    httpStatusCode = (int)HttpStatusCode.OK,
                    totalCount = PriceHasDecreased.Count,
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
        [HttpGet("images/product/{productId}")]
        public IActionResult GetProductImage(int productId)
        {
            try
            {
                var product = _productsInterface.GetProductsId(productId);

                if (product == null || string.IsNullOrEmpty(product.Image))
                {
                    return NotFound("Image not found!");
                }

                var imageBytes = _productsService.GetProductImageBytes(product.Image);
                return File(imageBytes, "image/jpeg"); // Điều chỉnh loại nội dung tùy thuộc
            }
            catch (Exception e)
            {
                return StatusCode(500, $"An error occurred: {e.Message}");
            }
        }

        [HttpGet("GetAllProduct")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult GetAllProduct()
        {
            try
            {
                var products = _productsService.GetAllProduct();
                return Ok(new XBaseResult
                {
                    data = products,
                    success = true,
                    httpStatusCode = (int)HttpStatusCode.OK,
                    totalCount = products.Count,
                    message = "List",
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new XBaseResult
                {
                    success = true,
                    httpStatusCode = (int)HttpStatusCode.BadRequest,
                    message = ex.Message
                });
            }
        }

        [HttpGet("product/{productID}")]
        public IActionResult getproductID(int productID)
        {
            var get = _productsInterface.GetProductsId(productID);
            return Ok(get);
        }

        [HttpGet("GetProductsWithinPriceRangeAndBrand")]
        //[Authorize]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult GetProductsWithinPriceRangeAndBrand([FromQuery] string productType, decimal minPrice, [FromQuery] decimal maxPrice, [FromQuery] string? brandsName)
        {
            try
            {
                if (minPrice < 0 || maxPrice < 0 || minPrice > maxPrice)
                {
                    return BadRequest(new XBaseResult
                    {
                        success = false,
                        httpStatusCode = (int)HttpStatusCode.BadRequest,
                        message = "Giá không hợp lệ. Vui lòng nhập khoảng giá đúng."
                    });
                }

                var getPrice = _productsService.GetProductsWithinPriceRangeAndBrand(productType, minPrice, maxPrice, brandsName);

                return Ok(new XBaseResult
                {
                    data = getPrice,
                    success = true,
                    httpStatusCode = (int)HttpStatusCode.OK,
                    totalCount = getPrice.Count(),
                    message = "Danh sách sản phẩm trong khoảng giá."
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new XBaseResult
                {
                    success = false,
                    httpStatusCode = (int)HttpStatusCode.BadRequest,
                    message = "Đã xảy ra lỗi: " + ex.Message
                });
            }
        }

        [HttpGet("GetProductsByNameAndColor")]
        //[Authorize]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult GetProductsByNameAndColor(string productName, string? color)
        {
            try
            {
                // Gọi phương thức lấy sản phẩm dựa trên productName và màu sắc
                var productDetails = _productsService.GetProductsByNameAndColor(productName, color);

                // Kiểm tra nếu không tìm thấy sản phẩm với màu cụ thể hoặc danh sách sản phẩm rỗng
                if ((productDetails.ProductDetail == null && productDetails.ProductDetails == null) ||
                    (productDetails.ProductDetails != null && !productDetails.ProductDetails.Any()))
                {
                    return BadRequest(new XBaseResult
                    {
                        success = false,
                        httpStatusCode = (int)HttpStatusCode.NotFound,
                        message = "Không tìm thấy sản phẩm nào với thông tin cung cấp."
                    });
                }

                // Trả về chi tiết sản phẩm và danh sách màu sắc
                return Ok(new XBaseResult
                {
                    data = productDetails,
                    success = true,
                    httpStatusCode = (int)HttpStatusCode.OK,
                    totalCount = productDetails.ProductDetails?.Count ?? 1, // Đếm số lượng sản phẩm
                    message = "Danh sách sản phẩm theo yêu cầu."
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

        [HttpGet("GetViewProductType")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetProductType(string productType)
        {
            try
            {
                var result = await _productsService.GetProductType(productType);
                return Ok(new XBaseResult
                {
                    data = result,
                    success = true,
                    httpStatusCode = (int)HttpStatusCode.OK,
                    totalCount = result.Count,
                    message = "GetViewProductType Successfully"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new XBaseResult
                {
                    success = false,
                    httpStatusCode = (int)HttpStatusCode.BadRequest,
                    message = "Đã xảy ra lỗi: " + ex.Message
                });
            }
        }

        [HttpDelete("Delete/{Id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> DeleteProduct(int Id)
        {
            try
            {
                var result = await _productsService.DeleteAsync(Id);
                if (result)
                {
                    return Ok(new XBaseResult
                    {
                        success = true,
                        httpStatusCode = (int)HttpStatusCode.OK,
                        message = "Product deleted successfully"
                    });
                }
                else
                {
                    return BadRequest(new XBaseResult
                    {
                        success = false,
                        httpStatusCode = (int)HttpStatusCode.BadRequest,
                        message = "Product could not be deleted"
                    });
                }
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

        [HttpGet("GetProductName")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetProductName(string productName)
        {
            try
            {
                var result = await _productsService.GetProductName(productName);
                return Ok(new XBaseResult
                {
                    data = result,
                    success = true,
                    httpStatusCode = (int)HttpStatusCode.OK,
                    totalCount = result.Count,
                    message = "GetProductName Successfully"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new XBaseResult
                {
                    data = null,
                    success = false,
                    httpStatusCode = (int)HttpStatusCode.BadRequest,
                    totalCount = 0,
                    message = ex.Message
                });
            }
        }

        [HttpGet("GetViewBoSuuTap")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetBoSuuTap(string productType)
        {
            try
            {
                var result = await _productsService.GetBoSuuTap(productType);
                return Ok(new XBaseResult
                {
                    data = result,
                    success = true,
                    httpStatusCode = (int)HttpStatusCode.OK,
                    totalCount = result.Count,
                    message = "GetBoSuuTap Successfully"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new XBaseResult
                {
                    success = false,
                    httpStatusCode = (int)HttpStatusCode.BadRequest,
                    message = "Đã xảy ra lỗi: " + ex.Message
                });
            }
        }

        [HttpGet("SearchKey")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> SearchKey(string keyWord)
        {
            try
            {
                var result = await _productsService.SearchKey(keyWord);
                return Ok(new XBaseResult
                {
                    data = result,
                    success = true,
                    httpStatusCode = (int)HttpStatusCode.OK,
                    totalCount = result.Count,
                    message = "keyWord Successfully"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new XBaseResult
                {
                    success = false,
                    httpStatusCode = (int)HttpStatusCode.BadRequest,
                    message = "Đã xảy ra lỗi: " + ex.Message
                });
            }
        }
    }
}