using System;
using System.Threading;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.Api.UseCases;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases;
using Moq;
using Xunit;

namespace GtMotive.Estimate.Microservice.UnitTests.Api.UseCases
{
    public sealed class CreateVehicleRequestHandlerTests
    {
        [Fact]
        public async Task HandleShouldExecuteUseCaseAndReturnPresenter()
        {
            // Arrange
            var useCase = new Mock<IUseCase<CreateVehicleInput>>();
            var presenter = new CreateVehiclePresenter();
            var handler = new CreateVehicleRequestHandler(useCase.Object, presenter);
            var request = new CreateVehicleRequest
            {
                VehicleId = "vehicle-1",
                Model = "model-1",
                ManufacturingDate = new DateOnly(2025, 1, 15),
            };

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            useCase.Verify(
                x => x.Execute(It.Is<CreateVehicleInput>(input =>
                    input.VehicleId == "vehicle-1" &&
                    input.Model == "model-1" &&
                    input.ManufacturingDate == new DateOnly(2025, 1, 15))),
                Times.Once);
            Assert.Same(presenter, result);
        }
    }
}
