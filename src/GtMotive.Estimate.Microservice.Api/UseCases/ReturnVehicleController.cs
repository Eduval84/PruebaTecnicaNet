using System;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GtMotive.Estimate.Microservice.Api.UseCases
{
    [ApiController]
    [Route("api/rentals")]
    public sealed class ReturnVehicleController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));

        [HttpPost("return")]
        [ProducesResponseType(typeof(ReturnVehicleOutput), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Return([FromBody] ReturnVehicleRequest request)
        {
            ArgumentNullException.ThrowIfNull(request);

            var presenter = await mediator.Send(request);
            return presenter.ActionResult;
        }
    }
}
