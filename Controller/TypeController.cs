using WebXeDapAPI.Common;
using WebXeDapAPI.Dto;
using WebXeDapAPI.Models.Enum;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;
using WebXeDapAPI.Helper;
using WebXeDapAPI.Service.Interfaces;

namespace WebXeDapAPI.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class TypeController : ControllerBase
    {
        private readonly ITypeIService _typeIService;
        private readonly Token _token;
        public TypeController(ITypeIService typeService,Token token)
        {
            _token = token;
            _typeIService = typeService;
        }
        [HttpPost("Create")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult CreateType(WebXeDapAPI.Models.Type type)
        {
            try
            {
                if(type == null)
                {
                    return Unauthorized("Invalid slide data");
                }
                var create = _typeIService.Create(type);
                return Ok(new XBaseResult
                {
                    data = create,
                    success = true,
                    httpStatusCode = (int)HttpStatusCode.OK,
                    totalCount = create.Id,
                    message = "CreateBook successfully"
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
        public IActionResult UpdateType([FromBody]TypeDto typeDto)
        {
            try
            {
                if (typeDto == null)
                {
                    return BadRequest("Invalid slide data");
                }
                var update = _typeIService.Update(typeDto);
                return Ok(new XBaseResult
                {
                    data = update,
                    success = true,
                    httpStatusCode = (int)HttpStatusCode.OK,
                    message = "Update successfully"
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
        public IActionResult DeleteType(int Id)
        {
            try
            {
                var delete = _typeIService.Delete(Id);
                return Ok(delete);
            }
            catch (Exception ex)
            {
                return BadRequest($"An error occurred: {ex.Message}");
            }
        }
    }
}
