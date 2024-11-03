using Microsoft.AspNetCore.Mvc;
using WebXeDapAPI.Dto;
using WebXeDapAPI.Service.Interfaces;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using WebXeDapAPI.Helper;
using WebXeDapAPI.Models;
using WebXeDapAPI.Common;
using System.Security.Claims;
using WebXeDapAPI.Models.Enum;

namespace WebXeDapAPI.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountingController : ControllerBase
    {
        private readonly IAccountingIService _accountingService;


        public AccountingController(IAccountingIService accountingService, Token token)
        {
            _accountingService = accountingService;
        }

        [HttpGet("All")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<DeliveryDto>> GetAll()
        {
            try
            {
                var payments = await _accountingService.FindAll();

                return Ok(new XBaseResult
                {
                    data = payments,
                    success = true,
                    httpStatusCode = (int)HttpStatusCode.OK,
                    message = "Get all payments successfully",
                    totalCount = payments?.Count ?? 0
                });
            }

            catch (Exception ex)
            {
                return BadRequest(new XBaseResult
                {
                    success = false,
                    httpStatusCode = (int)HttpStatusCode.BadRequest,
                    message = "An error occurred while getting payments: " + ex.Message
                });
            }
        }

    }
}
