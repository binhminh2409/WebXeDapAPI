
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebXeDapAPI.Common;
using WebXeDapAPI.Helper;
using WebXeDapAPI.Models;
using WebXeDapAPI.Models.Enum;
using WebXeDapAPI.Repository.Interface;

namespace WebXeDapAPI.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class StockController : ControllerBase
    {
        private readonly IStockIService _stockService;
        private readonly Token _token;
        public StockController(IStockIService stockService, Token token)
        {
            _token = token;
            _stockService = stockService;

        }

        [HttpPost("Create")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Create([FromBody] StockDto stockDto)
        {
            try
            {
                if (stockDto == null)
                {
                    return BadRequest("Dữ liệu slide không hợp lệ");
                }

                StockDto createdStock = await _stockService.CreateAsync(stockDto);
                return Ok(new XBaseResult
                {
                    data = createdStock,
                    success = true,
                    httpStatusCode = (int)HttpStatusCode.OK,
                    message = "Stock created"
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

        [HttpGet("All")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                List<StockDto> stockDtos = await _stockService.GetAllAsync();
                return Ok(new XBaseResult
                {
                    data = stockDtos,
                    success = true,
                    httpStatusCode = (int)HttpStatusCode.OK,
                    message = "Stock created"
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


        [HttpPost("Restock/Order")]
        [ProducesResponseType(200)]
        [Authorize(Roles = "ManagerMent")]
        [ProducesResponseType(400)]
        public async Task<IActionResult> RestockOrder([FromBody] List<InputStockDto> inputStockDtos)
        {
            try
            {
                var userIdClaim = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);

                int userId = int.Parse(userIdClaim.Value);
                foreach (var dto in inputStockDtos)
                {
                    dto.UserId = userId;
                }

                List<InputStockDto> createdInputStockDtos = await _stockService.RestockOrder(inputStockDtos);
                return Ok(new XBaseResult
                {
                    data = createdInputStockDtos,
                    success = true,
                    httpStatusCode = (int)HttpStatusCode.OK,
                    message = "Restock ordered successful"
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


        [HttpPost("Restock/Arrived/{batchNo_}")]
        [ProducesResponseType(200)]
        [Authorize(Roles = "ManagerMent")]
        [ProducesResponseType(400)]
        public async Task<IActionResult> RestockOrderArrived([FromRoute] string batchNo_)
        {
            try
            {
                List<InputStockDto> createdInputStockDtos = await _stockService.RestockOrderUpdateStatus(batchNo_, "ARRIVED");
                return Ok(new XBaseResult
                {
                    data = createdInputStockDtos,
                    success = true,
                    httpStatusCode = (int)HttpStatusCode.OK,
                    message = "Restock ordered successful"
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

        [HttpPost("Restock/Returned/{batchNo_}")]
        [ProducesResponseType(200)]
        [Authorize(Roles = "ManagerMent")]
        [ProducesResponseType(400)]
        public async Task<IActionResult> RestockOrderReturned([FromRoute] string batchNo_)
        {
            try
            {
                List<InputStockDto> createdInputStockDtos = await _stockService.RestockOrderUpdateStatus(batchNo_, "RETURNED");
                return Ok(new XBaseResult
                {
                    data = createdInputStockDtos,
                    success = true,
                    httpStatusCode = (int)HttpStatusCode.OK,
                    message = "Restock ordered successful"
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

        [HttpPost("Restock/Confirm/{batchNo_}")]
        [ProducesResponseType(200)]
        [Authorize(Roles = "ManagerMent")]
        [ProducesResponseType(400)]
        public async Task<IActionResult> RestockOrderSuccessful([FromRoute] string batchNo_)
        {
            try
            {
                List<InputStockDto> createdInputStockDtos = await _stockService.RestockOrderUpdateStatus(batchNo_, "SUCCESSFUL");
                return Ok(new XBaseResult
                {
                    data = createdInputStockDtos,
                    success = true,
                    httpStatusCode = (int)HttpStatusCode.OK,
                    message = "Restock ordered successful"
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

        [HttpGet("Restock/History")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> RestockHistory()
        {
            try
            {
                List<InputStockDto> inputStockDtos = await _stockService.RestockHistory();
                return Ok(new XBaseResult
                {
                    data = inputStockDtos,
                    success = true,
                    httpStatusCode = (int)HttpStatusCode.OK,
                    message = "Restock successful"
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
