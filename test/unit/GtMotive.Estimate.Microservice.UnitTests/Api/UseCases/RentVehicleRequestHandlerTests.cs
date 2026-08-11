using System.Threading;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.Api.UseCases;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases;
using Moq;
using Xunit;

namespace GtMotive.Estimate.Microservice.UnitTests.Api.UseCases
{
    public sealed class RentVehicleRequestHandlerTests
    {
        [Fact]
        public async Task HandleShouldExecuteUseCaseAndReturnPresenter()
        {
            // Arrange
            var useCase = new Mock<IUseCase<RentVehicleInput>>();
            var presenter = new RentVehiclePresenter();
            var handler = new RentVehicleRequestHandler(useCase.Object, presenter);
            var request = new RentVehicleRequest
            {
                CustomerId = "customer-1",
                VehicleId = "vehicle-1",
            };

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            useCase.Verify(
                x => x.Execute(It.Is<RentVehicleInput>(input =>
                    input.CustomerId == "customer-1" &&
                    input.VehicleId == "vehicle-1")),
                Times.Once);
            Assert.Same(presenter, result);
        }
    }
}
