using WebXeDapAPI.Common;
using WebXeDapAPI.Dto;
using WebXeDapAPI.Models.Enum;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Runtime.InteropServices;
using System.Security.Claims;
using WebXeDapAPI.Helper;
using WebXeDapAPI.Repository.Interface;
using WebXeDapAPI.Service.Interfaces;

namespace WebXeDapAPI.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class SlideController : ControllerBase
    {
        private readonly ISlideIService _slideIService;
        private readonly Token _token;
        private readonly ISlideInterface _slideInterface;
        public SlideController(ISlideIService slideIService, Token token, ISlideInterface slideInterface)
        {
            _slideInterface = slideInterface;
            _token = token;
            _slideIService = slideIService;
        }
        [HttpPost("Create")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult CreateSlide([FromForm] SlideDto slideDto)
        {
            try
            {
                if (slideDto == null)
                {
                    return BadRequest("Dữ liệu slide không hợp lệ");
                }

                var createdSlide = _slideIService.Create(slideDto).Result;
                return Ok(new XBaseResult
                {
                    data = createdSlide,
                    success = true,
                    httpStatusCode = (int)HttpStatusCode.OK,
                    message = "Tạo slide thành công"
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
        /// <summary>
        /// Update slide Url Sort
        /// </summary>
        /// <param name="updateSlideDto"></param>
        /// <returns></returns>
        [HttpPut("Update")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult UpdateSlide([FromBody] UpdateSlideDto updateSlideDto)
        {
            try
            {
                if (updateSlideDto == null)
                {
                    return BadRequest("Invalid slide data");
                }
                var update = _slideIService.Update(updateSlideDto);
                return Ok(new XBaseResult
                {
                    data = update,
                    success = true,
                    httpStatusCode = (int)HttpStatusCode.OK,
                    totalCount = update.Count(),
                    message = "Update Slide successfully"
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
        [HttpDelete("Delete")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult deleteSlide(int Id)
        {
            if (Id == null)
            {
                throw new ArgumentNullException(nameof(Id), "Slide not found");
            }
            var delete = _slideIService.Delete(Id);
            return Ok(delete);
        }
        [HttpGet("GetList")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult getSlide()
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
                var getlist = _slideInterface.GetSlides();
                return Ok(new XBaseResult
                {
                    data = getlist,
                    success = true,
                    httpStatusCode = (int)HttpStatusCode.OK,
                    totalCount = getlist.Count(),
                    message = "Getlist Successfully"
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
        [HttpGet("images/slide/{slideId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult GetProductImage(int slideId)
        {
            try
            {
                var slide = _slideInterface.GetSlideImage(slideId);

                if (slide == null || string.IsNullOrEmpty(slide.Image))
                {
                    return NotFound("Image not found!");
                }

                var imageBytes = _slideIService.GetSileBytesImage(slide.Image);
                return File(imageBytes, "image/jpeg"); // Điều chỉnh loại nội dung tùy thuộc
            }
            catch (Exception e)
            {
                return StatusCode(500, $"An error occurred: {e.Message}");
            }
        }
        [HttpGet("images/slide/4")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult GetProductImageid4(int slideId = 4)
        {
            try
            {
                var slide = _slideInterface.GetSlideImage(slideId);

                if (slide == null || string.IsNullOrEmpty(slide.Image))
                {
                    return NotFound("Image not found!");
                }

                var imageBytes = _slideIService.GetSlideBytesImageid4(slide.Image);
                return File(imageBytes, "image/jpeg"); // Adjust content type as needed
            }
            catch (Exception e)
            {
                return StatusCode(500, $"An error occurred: {e.Message}");
            }
        }
        [HttpGet("images/slide/5")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult GetProductImageid5(int slideId = 5)
        {
            try
            {
                var slide = _slideInterface.GetSlideImage(slideId);

                if (slide == null || string.IsNullOrEmpty(slide.Image))
                {
                    return NotFound("Image not found!");
                }

                var imageBytes = _slideIService.GetSlideBytesImageid4(slide.Image);
                return File(imageBytes, "image/jpeg"); // Adjust content type as needed
            }
            catch (Exception e)
            {
                return StatusCode(500, $"An error occurred: {e.Message}");
            }
        }

        [HttpGet("GetById")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult GetById(int Id)
        {
            try
            {
                var getlist = _slideInterface.GetById(Id);
                return Ok(new XBaseResult
                {
                    data = getlist,
                    success = true,
                    httpStatusCode = (int)HttpStatusCode.OK,
                    totalCount = getlist.Count(),
                    message = "Getlist Successfully"
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
