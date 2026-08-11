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
    public sealed class ListAvailableVehiclesController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));

        [HttpGet("available")]
        [ProducesResponseType(typeof(ListAvailableVehiclesOutput), StatusCodes.Status200OK)]
        public async Task<IActionResult> ListAvailable()
        {
            var presenter = await mediator.Send(new ListAvailableVehiclesRequest());
            return presenter.ActionResult;
        }
    }
}
