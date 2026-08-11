using System.Threading;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.Api.UseCases;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases;
using Moq;
using Xunit;

namespace GtMotive.Estimate.Microservice.UnitTests.Api.UseCases
{
    public sealed class ListAvailableVehiclesRequestHandlerTests
    {
        [Fact]
        public async Task HandleShouldExecuteUseCaseAndReturnPresenter()
        {
            // Arrange
            var useCase = new Mock<IUseCase<ListAvailableVehiclesInput>>();
            var presenter = new ListAvailableVehiclesPresenter();
            var handler = new ListAvailableVehiclesRequestHandler(useCase.Object, presenter);
            var request = new ListAvailableVehiclesRequest();

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            useCase.Verify(x => x.Execute(It.IsAny<ListAvailableVehiclesInput>()), Times.Once);
            Assert.Same(presenter, result);
        }
    }
}
