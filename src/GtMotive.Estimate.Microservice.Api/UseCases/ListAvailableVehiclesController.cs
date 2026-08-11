using System;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GtMotive.Estimate.Microservice.Api.UseCases
{
    [ApiController]
    [Route("api/vehicles")]
    public sealed class ListAvailableVehiclesController(IUseCase<ListAvailableVehiclesInput> listAvailableVehiclesUseCase, ListAvailableVehiclesPresenter listAvailableVehiclesPresenter) : ControllerBase
    {
        private readonly IUseCase<ListAvailableVehiclesInput> listAvailableVehiclesUseCase = listAvailableVehiclesUseCase ?? throw new ArgumentNullException(nameof(listAvailableVehiclesUseCase));
        private readonly ListAvailableVehiclesPresenter listAvailableVehiclesPresenter = listAvailableVehiclesPresenter ?? throw new ArgumentNullException(nameof(listAvailableVehiclesPresenter));

        [HttpGet("available")]
        [ProducesResponseType(typeof(ListAvailableVehiclesOutput), StatusCodes.Status200OK)]
        public async Task<IActionResult> ListAvailable()
        {
            await listAvailableVehiclesUseCase.Execute(new ListAvailableVehiclesInput());
            return listAvailableVehiclesPresenter.ActionResult;
        }
    }
}
