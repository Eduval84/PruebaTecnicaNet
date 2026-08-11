using System;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases;
using GtMotive.Estimate.Microservice.Domain;
using GtMotive.Estimate.Microservice.Domain.Interfaces;
using Moq;
using Xunit;

namespace GtMotive.Estimate.Microservice.UnitTests.ApplicationCore
{
    public class ReturnVehicleUseCaseTests
    {
        [Fact]
        public async Task ExecuteShouldReturnVehicleAndPersistChangesWhenDataIsValid()
        {
            // Arrange
            var customerRepository = new Mock<ICustomerRepository>();
            var vehicleRepository = new Mock<IVehicleRepository>();
            var unitOfWork = new Mock<IUnitOfWork>();
            var presenter = new Mock<IReturnVehicleOutputPort>();
            var useCase = new ReturnVehicleUseCase(customerRepository.Object, vehicleRepository.Object, unitOfWork.Object, presenter.Object);
            var input = new ReturnVehicleInput("customer-1", "vehicle-1");
            var customer = Customer.Create("customer-1");
            var vehicle = Vehicle.Create("vehicle-1", "model-1", ManufacturingDate.Create(new DateOnly(2024, 1, 1)));

            customer.StartRental("vehicle-1");
            vehicle.Rent();

            customerRepository
                .Setup(x => x.GetById("customer-1"))
                .ReturnsAsync(customer);
            vehicleRepository
                .Setup(x => x.GetById("vehicle-1"))
                .ReturnsAsync(vehicle);

            // Act
            await useCase.Execute(input);

            // Assert
            customerRepository.Verify(x => x.Update(customer), Times.Once);
            vehicleRepository.Verify(x => x.Update(vehicle), Times.Once);
            unitOfWork.Verify(x => x.Save(), Times.Once);
            presenter.Verify(x => x.StandardHandle(It.Is<ReturnVehicleOutput>(output => output.CustomerId == "customer-1" && output.VehicleId == "vehicle-1")), Times.Once);
        }
    }
}
