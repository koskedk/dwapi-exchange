using System;
using System.Threading.Tasks;
using Dwapi.Exchange.Core.Application.Definitions.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace Dwapi.Exchange.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class DatasetController : ControllerBase
    {
        private readonly IMediator _mediator;

        public DatasetController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult> Get(string code, string name, int pageNumber, int pageSize)
        {
            try
            {
                var request = new GetExtract(code, name, pageNumber, pageSize);

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
