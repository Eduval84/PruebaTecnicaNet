using System;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GtMotive.Estimate.Microservice.Api.UseCases
{
    [ApiController]
    [Route("api/rentals")]
    public sealed class ReturnVehicleController(IUseCase<ReturnVehicleInput> returnVehicleUseCase, ReturnVehiclePresenter returnVehiclePresenter) : ControllerBase
    {
        private readonly IUseCase<ReturnVehicleInput> returnVehicleUseCase = returnVehicleUseCase ?? throw new ArgumentNullException(nameof(returnVehicleUseCase));
        private readonly ReturnVehiclePresenter returnVehiclePresenter = returnVehiclePresenter ?? throw new ArgumentNullException(nameof(returnVehiclePresenter));

        [HttpPost("return")]
        [ProducesResponseType(typeof(ReturnVehicleOutput), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Return([FromBody] ReturnVehicleRequest request)
        {
            ArgumentNullException.ThrowIfNull(request);

            await returnVehicleUseCase.Execute(new ReturnVehicleInput(request.CustomerId, request.VehicleId));
            return returnVehiclePresenter.ActionResult;
        }
    }
}
