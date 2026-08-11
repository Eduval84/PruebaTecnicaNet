using System;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GtMotive.Estimate.Microservice.Api.UseCases
{
    [ApiController]
    [Route("api/vehicles")]
    public sealed class CreateVehicleController(IUseCase<CreateVehicleInput> createVehicleUseCase, CreateVehiclePresenter createVehiclePresenter) : ControllerBase
    {
        private readonly IUseCase<CreateVehicleInput> createVehicleUseCase = createVehicleUseCase ?? throw new ArgumentNullException(nameof(createVehicleUseCase));
        private readonly CreateVehiclePresenter createVehiclePresenter = createVehiclePresenter ?? throw new ArgumentNullException(nameof(createVehiclePresenter));

        [HttpPost("create")]
        [ProducesResponseType(typeof(CreateVehicleOutput), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateVehicleRequest request)
        {
            ArgumentNullException.ThrowIfNull(request);

            await createVehicleUseCase.Execute(new CreateVehicleInput(request.VehicleId, request.Model, request.ManufacturingDate));
            return createVehiclePresenter.ActionResult;
        }
    }
}
