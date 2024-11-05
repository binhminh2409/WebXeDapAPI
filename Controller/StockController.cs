
using System.Diagnostics.CodeAnalysis;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using WebXeDapAPI.Common;
using WebXeDapAPI.Helper;
using WebXeDapAPI.Models;
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

        
        [HttpPost("Restock")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Restock([FromBody] List<InputStockDto> inputStockDtos)
        {
            try
            {
                List<InputStockDto> createdInputStockDtos = await _stockService.Restock(inputStockDtos);
                return Ok(new XBaseResult
                {
                    data = createdInputStockDtos,
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