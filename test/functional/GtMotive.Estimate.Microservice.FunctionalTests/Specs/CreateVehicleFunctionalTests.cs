using System;
using System.Threading.Tasks;
using FluentAssertions;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases;
using GtMotive.Estimate.Microservice.FunctionalTests.Infrastructure;
using Xunit;

namespace GtMotive.Estimate.Microservice.FunctionalTests.Specs
{
    public sealed class CreateVehicleFunctionalTests(CompositionRootTestFixture fixture) : FunctionalTestBase(fixture)
    {
        [Fact]
        public async Task ExecuteShouldPersistVehicleInInMemoryRepository()
        {
            // Arrange
            var input = new CreateVehicleInput("vehicle-functional-1", "Model Functional", DateOnly.FromDateTime(DateTime.UtcNow.Date.AddYears(-1)));

            // Act
            await Fixture.UsingRepository<IUseCase<CreateVehicleInput>>(useCase => useCase.Execute(input));

            // Assert
            await Fixture.UsingRepository<IVehicleRepository>(async vehicleRepository =>
            {
                var persistedVehicle = await vehicleRepository.GetById(input.VehicleId);

                persistedVehicle.Should().NotBeNull();
                persistedVehicle!.Model.Should().Be(input.Model);
                persistedVehicle.ManufacturingDate.Value.Should().Be(input.ManufacturingDate);
            });
        }
    }
}
