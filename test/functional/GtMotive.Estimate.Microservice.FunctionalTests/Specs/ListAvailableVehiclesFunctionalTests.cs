using System;
using System.Threading.Tasks;
using FluentAssertions;
using GtMotive.Estimate.Microservice.Api.UseCases;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases;
using GtMotive.Estimate.Microservice.Domain;
using GtMotive.Estimate.Microservice.FunctionalTests.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace GtMotive.Estimate.Microservice.FunctionalTests.Specs
{
    public sealed class ListAvailableVehiclesFunctionalTests(CompositionRootTestFixture fixture) : FunctionalTestBase(fixture)
    {
        [Fact]
        public async Task ExecuteShouldReturnOnlyAvailableVehicles()
        {
            // Arrange
            const string customerId = "customer-list-available";
            const string availableVehicleId = "vehicle-list-available";
            const string rentedVehicleId = "vehicle-list-rented";

            await Fixture.UsingRepository<ICustomerRepository>(customerRepository =>
                customerRepository.Update(Customer.Create(customerId)));

            await Fixture.UsingRepository<IUseCase<CreateVehicleInput>>(useCase =>
                useCase.Execute(new CreateVehicleInput(availableVehicleId, "Model Available", DateOnly.FromDateTime(DateTime.UtcNow.Date.AddYears(-1)))));

            await Fixture.UsingRepository<IUseCase<CreateVehicleInput>>(useCase =>
                useCase.Execute(new CreateVehicleInput(rentedVehicleId, "Model Rented", DateOnly.FromDateTime(DateTime.UtcNow.Date.AddYears(-2)))));

            await Fixture.UsingRepository<IUseCase<RentVehicleInput>>(useCase =>
                useCase.Execute(new RentVehicleInput(customerId, rentedVehicleId)));

            ListAvailableVehiclesOutput output = null;

            // Act
            await Fixture.UsingScope(async serviceProvider =>
            {
                var useCase = serviceProvider.GetRequiredService<IUseCase<ListAvailableVehiclesInput>>();
                var presenter = serviceProvider.GetRequiredService<ListAvailableVehiclesPresenter>();

                await useCase.Execute(new ListAvailableVehiclesInput());

                var result = Assert.IsType<OkObjectResult>(presenter.ActionResult);
                output = Assert.IsType<ListAvailableVehiclesOutput>(result.Value);
            });

            // Assert
            output.Should().NotBeNull();
            output!.Vehicles.Should().Contain(x => x.VehicleId == availableVehicleId);
            output.Vehicles.Should().NotContain(x => x.VehicleId == rentedVehicleId);
        }
    }
}
