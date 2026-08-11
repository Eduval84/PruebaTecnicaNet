using GtMotive.Estimate.Microservice.ApplicationCore.UseCases;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GtMotive.Estimate.Microservice.Api.UseCases
{
    public sealed class ListAvailableVehiclesPresenter : IListAvailableVehiclesOutputPort, IWebApiPresenter
    {
        public IActionResult ActionResult { get; private set; } = new StatusCodeResult(StatusCodes.Status500InternalServerError);

        public void StandardHandle(ListAvailableVehiclesOutput useCaseOutput)
        {
            ActionResult = new OkObjectResult(useCaseOutput);
        }
    }
}
