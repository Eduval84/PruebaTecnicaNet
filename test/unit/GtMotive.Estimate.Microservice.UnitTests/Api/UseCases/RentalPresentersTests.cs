using GtMotive.Estimate.Microservice.Api.UseCases;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace GtMotive.Estimate.Microservice.UnitTests.Api.UseCases
{
    public class RentalPresentersTests
    {
        [Fact]
        public void RentPresenterShouldMapStandardHandleToOkResult()
        {
            // Arrange
            var presenter = new RentVehiclePresenter();
            var output = new RentVehicleOutput("customer-1", "vehicle-1");

            // Act
            presenter.StandardHandle(output);

            // Assert
            var result = Assert.IsType<OkObjectResult>(presenter.ActionResult);
            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
            var payload = Assert.IsType<RentVehicleOutput>(result.Value);
            Assert.Equal("customer-1", payload.CustomerId);
            Assert.Equal("vehicle-1", payload.VehicleId);
        }

        [Fact]
        public void RentPresenterShouldMapNotFoundHandleToNotFoundResult()
        {
            // Arrange
            var presenter = new RentVehiclePresenter();

            // Act
            presenter.NotFoundHandle("Vehicle 'vehicle-404' was not found for rent.");

            // Assert
            var result = Assert.IsType<NotFoundObjectResult>(presenter.ActionResult);
            Assert.Equal(StatusCodes.Status404NotFound, result.StatusCode);
            var problemDetails = Assert.IsType<ProblemDetails>(result.Value);
            Assert.Equal(StatusCodes.Status404NotFound, problemDetails.Status);
            Assert.Equal("Not Found", problemDetails.Title);
            Assert.Equal("Vehicle 'vehicle-404' was not found for rent.", problemDetails.Detail);
        }

        [Fact]
        public void ReturnPresenterShouldMapStandardHandleToOkResult()
        {
            // Arrange
            var presenter = new ReturnVehiclePresenter();
            var output = new ReturnVehicleOutput("customer-1", "vehicle-1");

            // Act
            presenter.StandardHandle(output);

            // Assert
            var result = Assert.IsType<OkObjectResult>(presenter.ActionResult);
            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
            var payload = Assert.IsType<ReturnVehicleOutput>(result.Value);
            Assert.Equal("customer-1", payload.CustomerId);
            Assert.Equal("vehicle-1", payload.VehicleId);
        }

        [Fact]
        public void ReturnPresenterShouldMapNotFoundHandleToNotFoundResult()
        {
            // Arrange
            var presenter = new ReturnVehiclePresenter();

            // Act
            presenter.NotFoundHandle("Vehicle 'vehicle-404' was not found for return.");

            // Assert
            var result = Assert.IsType<NotFoundObjectResult>(presenter.ActionResult);
            Assert.Equal(StatusCodes.Status404NotFound, result.StatusCode);
            var problemDetails = Assert.IsType<ProblemDetails>(result.Value);
            Assert.Equal(StatusCodes.Status404NotFound, problemDetails.Status);
            Assert.Equal("Not Found", problemDetails.Title);
            Assert.Equal("Vehicle 'vehicle-404' was not found for return.", problemDetails.Detail);
        }
    }
}
