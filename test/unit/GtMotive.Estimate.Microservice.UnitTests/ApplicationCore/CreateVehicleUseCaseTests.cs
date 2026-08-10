using System;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases;
using GtMotive.Estimate.Microservice.Domain;
using GtMotive.Estimate.Microservice.Domain.Interfaces;
using Moq;
using Xunit;

namespace GtMotive.Estimate.Microservice.UnitTests.ApplicationCore;

public class CreateVehicleUseCaseTests
{
    [Fact]
    public async Task ExecuteShouldPersistNewVehicleAndNotifyPresenterWhenRequestIsValid()
    {
        // Arrange
        var vehicleRepository = new Mock<IVehicleRepository>();
        var unitOfWork = new Mock<IUnitOfWork>();
        var presenter = new Mock<ICreateVehicleOutputPort>();
        var useCase = new CreateVehicleUseCase(vehicleRepository.Object, unitOfWork.Object, presenter.Object);
        var input = new CreateVehicleInput("vehicle-1", "model-1", new DateOnly(2024, 1, 1));

        // Act
        await useCase.Execute(input);

        // Assert
        vehicleRepository.Verify(x => x.Add(It.Is<Vehicle>(vehicle => vehicle.Id == "vehicle-1" && vehicle.Model == "model-1")), Times.Once);
        unitOfWork.Verify(x => x.Save(), Times.Once);
        presenter.Verify(x => x.StandardHandle(It.Is<CreateVehicleOutput>(output => output.VehicleId == "vehicle-1")), Times.Once);
    }
}
