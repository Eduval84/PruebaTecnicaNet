using System;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GtMotive.Estimate.Microservice.Api.UseCases
{
    [ApiController]
    [Route("api/vehicles")]
    public sealed class CreateVehicleController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));

        [HttpPost("create")]
        [ProducesResponseType(typeof(CreateVehicleOutput), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateVehicleRequest request)
        {
            ArgumentNullException.ThrowIfNull(request);

            var presenter = await mediator.Send(request);
            return presenter.ActionResult;
        }
    }
}
