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
        public async Task ExecuteShouldNotifyNotFoundAndStopWhenCustomerDoesNotExist()
        {
            // Arrange
            var customerRepository = new Mock<ICustomerRepository>();
            var vehicleRepository = new Mock<IVehicleRepository>(MockBehavior.Strict);
            var unitOfWork = new Mock<IUnitOfWork>();
            var presenter = new Mock<IReturnVehicleOutputPort>();
            var useCase = new ReturnVehicleUseCase(customerRepository.Object, vehicleRepository.Object, unitOfWork.Object, presenter.Object);
            var input = new ReturnVehicleInput("customer-404", "vehicle-1");

            customerRepository
                .Setup(x => x.GetById("customer-404"))
                .ReturnsAsync((Customer)null);

            // Act
            await useCase.Execute(input);

            // Assert
            vehicleRepository.Verify(x => x.GetById(It.IsAny<string>()), Times.Never);
            customerRepository.Verify(x => x.Update(It.IsAny<Customer>()), Times.Never);
            unitOfWork.Verify(x => x.Save(), Times.Never);
            presenter.Verify(x => x.NotFoundHandle("Customer 'customer-404' was not found."), Times.Once);
            presenter.Verify(x => x.StandardHandle(It.IsAny<ReturnVehicleOutput>()), Times.Never);
        }

        [Fact]
        public async Task ExecuteShouldNotifyNotFoundWhenVehicleDoesNotExist()
        {
            // Arrange
            var customerRepository = new Mock<ICustomerRepository>();
            var vehicleRepository = new Mock<IVehicleRepository>();
            var unitOfWork = new Mock<IUnitOfWork>();
            var presenter = new Mock<IReturnVehicleOutputPort>();
            var useCase = new ReturnVehicleUseCase(customerRepository.Object, vehicleRepository.Object, unitOfWork.Object, presenter.Object);
            var input = new ReturnVehicleInput("customer-1", "vehicle-404");
            var customer = Customer.Create("customer-1");

            customerRepository
                .Setup(x => x.GetById("customer-1"))
                .ReturnsAsync(customer);
            vehicleRepository
                .Setup(x => x.GetById("vehicle-404"))
                .ReturnsAsync((Vehicle)null);

            // Act
            await useCase.Execute(input);

            // Assert
            customerRepository.Verify(x => x.Update(It.IsAny<Customer>()), Times.Never);
            vehicleRepository.Verify(x => x.Update(It.IsAny<Vehicle>()), Times.Never);
            unitOfWork.Verify(x => x.Save(), Times.Never);
            presenter.Verify(x => x.NotFoundHandle("Vehicle 'vehicle-404' was not found for return."), Times.Once);
            presenter.Verify(x => x.StandardHandle(It.IsAny<ReturnVehicleOutput>()), Times.Never);
        }

        [Fact]
        public async Task ExecuteShouldNotifyNotFoundWhenVehicleIsNotRented()
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

            customerRepository
                .Setup(x => x.GetById("customer-1"))
                .ReturnsAsync(customer);
            vehicleRepository
                .Setup(x => x.GetById("vehicle-1"))
                .ReturnsAsync(vehicle);

            // Act
            await useCase.Execute(input);

            // Assert
            customerRepository.Verify(x => x.Update(It.IsAny<Customer>()), Times.Never);
            vehicleRepository.Verify(x => x.Update(It.IsAny<Vehicle>()), Times.Never);
            unitOfWork.Verify(x => x.Save(), Times.Never);
            presenter.Verify(x => x.NotFoundHandle("Vehicle 'vehicle-1' does not have an active rental."), Times.Once);
            presenter.Verify(x => x.StandardHandle(It.IsAny<ReturnVehicleOutput>()), Times.Never);
        }

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
