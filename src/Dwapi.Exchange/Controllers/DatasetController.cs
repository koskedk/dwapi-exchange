using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Dwapi.Exchange.Core.Application.Definitions.Queries;
using Dwapi.Exchange.Core.Domain.Definitions.Dtos;
using Dwapi.Exchange.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace Dwapi.Exchange.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DatasetController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IHttpContextAccessor _accessor;
        private readonly IMapper _mapper;

        public DatasetController(IMediator mediator, IHttpContextAccessor accessor, IMapper mapper)
        {
            _mediator = mediator;
            _accessor = accessor;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult> Get([FromQuery] EmrReqDto req)
        {
            #region Move to middleware

            try
            {
                var client = User.Claims.FirstOrDefault(c => c.Type == "client_id")?.Value;
                var ip = _accessor.HttpContext?.Connection?.RemoteIpAddress?.ToString();

                Log.Warning($"Client:{client}");
                Log.Warning($"ClientIp:{ip}");
                Log.Warning($"Request:{Request.Path}");
                Log.Warning($"Query:{Request.QueryString}");
                Log.Warning(new string('-',50));
            }
            catch (Exception e)
            {
                Log.Error(e,"Request error");
            }

            #endregion


            try
            {

                var request = new GetExtract(req.code, req.name, req.pageNumber, req.pageSize,req.siteCode);

                var results = await _mediator.Send(request);

                if (results.IsSuccess)
                    return Ok(results.Value);

                throw new Exception(results.Error);
            }
            catch (Exception e)
            {
                var msg = $"Error loading Dataset ";
                Log.Error(e, msg);
                return StatusCode(500, $"{msg} {e.Message}");
            }
        }
        
        [HttpGet("v2")]
        public async Task<ActionResult> GetEmr([FromQuery] EmrReqDto req)
        {
            #region Move to middleware

            try
            {
                var client = User.Claims.FirstOrDefault(c => c.Type == "client_id")?.Value;
                var ip = _accessor.HttpContext?.Connection?.RemoteIpAddress?.ToString();

                Log.Warning($"Client:{client}");
                Log.Warning($"ClientIp:{ip}");
                Log.Warning($"Request:{Request.Path}");
                Log.Warning($"Query:{Request.QueryString}");
                Log.Warning(new string('-',50));
            }
            catch (Exception e)
            {
                Log.Error(e,"Request error");
            }

            #endregion


            try
            {
                var emrRequest = _mapper.Map<EmrRequestDto>(req);
                var request = new GetEmrExtract(emrRequest);

                var results = await _mediator.Send(request);

                if (results.IsSuccess)
                    return Ok(results.Value);

                throw new Exception(results.Error);
            }
            catch (Exception e)
            {
                var msg = $"Error loading Dataset ";
                Log.Error(e, msg);
                return StatusCode(500, $"{msg} {e.Message}");
            }
        }
        

        [HttpGet("Profile")]
        public async Task<ActionResult> GetProfile([FromQuery] ReqDto req)
        {
            #region Move to middleware

            try
            {
                var client = User.Claims.FirstOrDefault(c => c.Type == "client_id")?.Value;
                var ip = _accessor.HttpContext?.Connection?.RemoteIpAddress?.ToString();

                Log.Warning($"Client:{client}");
                Log.Warning($"ClientIp:{ip}");
                Log.Warning($"Request:{Request.Path}");
                Log.Warning($"Query:{Request.QueryString}");
                Log.Warning(new string('-',50));
            }
            catch (Exception e)
            {
                Log.Error(e,"Request error");
            }

            #endregion


            try
            {

                var request = new GetProfileExtract(req.code, "Profile", req.pageNumber, req.pageSize,req.siteCode,req.county,req.gender,req.age);

                var results = await _mediator.Send(request);

                if (results.IsSuccess)
                    return Ok(results.Value);

                throw new Exception(results.Error);
            }
            catch (Exception e)
            {
                var msg = $"Error loading Dataset ";
                Log.Error(e, msg);
                return StatusCode(500, $"{msg} {e.Message}");
            }
        }
    }
}
