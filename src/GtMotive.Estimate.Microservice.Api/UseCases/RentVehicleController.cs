using System;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GtMotive.Estimate.Microservice.Api.UseCases
{
    [ApiController]
    [Route("api/rentals")]
    public sealed class RentVehicleController(IUseCase<RentVehicleInput> rentVehicleUseCase, RentVehiclePresenter rentVehiclePresenter) : ControllerBase
    {
        private readonly IUseCase<RentVehicleInput> rentVehicleUseCase = rentVehicleUseCase ?? throw new ArgumentNullException(nameof(rentVehicleUseCase));
        private readonly RentVehiclePresenter rentVehiclePresenter = rentVehiclePresenter ?? throw new ArgumentNullException(nameof(rentVehiclePresenter));

        [HttpPost("rent")]
        [ProducesResponseType(typeof(RentVehicleOutput), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Rent([FromBody] RentVehicleRequest request)
        {
            ArgumentNullException.ThrowIfNull(request);

            await rentVehicleUseCase.Execute(new RentVehicleInput(request.CustomerId, request.VehicleId));
            return rentVehiclePresenter.ActionResult;
        }
    }
}
