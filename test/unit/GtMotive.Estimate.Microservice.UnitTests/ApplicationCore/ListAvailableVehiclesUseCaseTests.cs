using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases;
using GtMotive.Estimate.Microservice.Domain;
using Moq;
using Xunit;

namespace GtMotive.Estimate.Microservice.UnitTests.ApplicationCore
{
    public class ListAvailableVehiclesUseCaseTests
    {
        [Fact]
        public async Task ExecuteShouldReturnOnlyAvailableVehicles()
        {
            // Arrange
            var vehicleRepository = new Mock<IVehicleRepository>();
            var presenter = new Mock<IListAvailableVehiclesOutputPort>();
            var useCase = new ListAvailableVehiclesUseCase(vehicleRepository.Object, presenter.Object);
            var input = new ListAvailableVehiclesInput();
            var vehicles = new List<Vehicle>
            {
                Vehicle.Create("vehicle-1", "model-1", ManufacturingDate.Create(new DateOnly(2024, 1, 1))),
                Vehicle.Create("vehicle-2", "model-2", ManufacturingDate.Create(new DateOnly(2023, 1, 1))),
            };

            vehicleRepository
                .Setup(x => x.ListAvailable())
                .ReturnsAsync(vehicles);

            // Act
            await useCase.Execute(input);

            // Assert
            vehicleRepository.Verify(x => x.ListAvailable(), Times.Once);
            presenter.Verify(x => x.StandardHandle(It.Is<ListAvailableVehiclesOutput>(output => output.Vehicles.Count == 2)), Times.Once);
        }
    }
}
