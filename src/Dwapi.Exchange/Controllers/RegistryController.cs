using System;
using System.Threading.Tasks;
using Dwapi.Exchange.Core.Application.Definitions.Commands;
using Dwapi.Exchange.Core.Application.Definitions.Queries;
using Dwapi.Exchange.Core.Domain.Definitions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace Dwapi.Exchange.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class RegistryController : ControllerBase
    {
        private readonly IMediator _mediator;

        public RegistryController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult> Get()
        {
            try
            {
                var results = await _mediator.Send(new GetRegistry());

                if (results.IsSuccess)
                    return Ok(results.Value);

                throw new Exception(results.Error);
            }
            catch (Exception e)
            {
                var msg = $"Error loading {nameof(Registry)} information";
                Log.Error(e, msg);
                return StatusCode(500, $"{msg} {e.Message}");
            }
        }

        [HttpPost("Refresh")]
        public async Task<ActionResult> Refresh([FromBody] RefreshIndex command)
        {
            try
            {
                var results = await _mediator.Send(command);

                if (results.IsSuccess)
                    return Ok(new
                    {
                        RefresStatus = $"Registry {command.Code} Updated successfully"
                    });

                throw new Exception(results.Error);
            }
            catch (Exception e)
            {
                var msg = $"Error loading {nameof(Registry)} information";
                Log.Error(e, msg);
                return StatusCode(500, $"{msg} {e.Message}");
            }
        }
    }
}
