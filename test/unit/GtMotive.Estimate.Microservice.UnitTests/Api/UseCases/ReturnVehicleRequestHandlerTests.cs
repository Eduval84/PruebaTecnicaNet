using System.Threading;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.Api.UseCases;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases;
using Moq;
using Xunit;

namespace GtMotive.Estimate.Microservice.UnitTests.Api.UseCases
{
    public sealed class ReturnVehicleRequestHandlerTests
    {
        [Fact]
        public async Task HandleShouldExecuteUseCaseAndReturnPresenter()
        {
            // Arrange
            var useCase = new Mock<IUseCase<ReturnVehicleInput>>();
            var presenter = new ReturnVehiclePresenter();
            var handler = new ReturnVehicleRequestHandler(useCase.Object, presenter);
            var request = new ReturnVehicleRequest
            {
                CustomerId = "customer-1",
                VehicleId = "vehicle-1",
            };

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            useCase.Verify(
                x => x.Execute(It.Is<ReturnVehicleInput>(input =>
                    input.CustomerId == "customer-1" &&
                    input.VehicleId == "vehicle-1")),
                Times.Once);
            Assert.Same(presenter, result);
        }
    }
}
