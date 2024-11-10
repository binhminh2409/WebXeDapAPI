using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using WebXeDapAPI.Common;
using WebXeDapAPI.Dto;
using WebXeDapAPI.Models;
using WebXeDapAPI.Service;
using WebXeDapAPI.Service.Interfaces;

namespace WebXeDapAPI.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdsController : ControllerBase
    {
        private readonly IAdsIService _adsService;
        public AdsController(IAdsIService adsService)
        {
            _adsService = adsService;
        }

        [HttpPost("Create")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult CreateAds([FromForm]AdsDto adsDto)
        {
            try
            {
                if (adsDto == null)
                {
                    return Unauthorized("Invalid slide data");
                }
                var create = _adsService.Create(adsDto);
                return Ok(new XBaseResult
                {
                    data = create,
                    success = true,
                    httpStatusCode = (int)HttpStatusCode.OK,
                    message = "Ads Successfully"
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

        [HttpGet("GetAll")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult GetAllAds ()
        {
            try
            {
                var getAllAds = _adsService.GetAll();
                return Ok(new XBaseResult
                {
                    data = getAllAds,
                    success = true,
                    httpStatusCode = (int)HttpStatusCode.OK,
                    totalCount = getAllAds.Count,
                    message = "GetAllAds Successfully"
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
