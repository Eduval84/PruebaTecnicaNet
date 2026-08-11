using System;
using System.Linq;
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
    public sealed class ReturnVehicleAvailabilityFunctionalTests(CompositionRootTestFixture fixture) : FunctionalTestBase(fixture)
    {
        [Fact]
        public async Task ExecuteShouldMakeVehicleAvailableAgainAfterReturn()
        {
            // Arrange
            var suffix = Guid.NewGuid().ToString("N");
            var customerId = $"customer-return-{suffix}";
            var vehicleId = $"vehicle-return-{suffix}";

            await Fixture.UsingRepository<ICustomerRepository>(customerRepository =>
                customerRepository.Update(Customer.Create(customerId)));

            await Fixture.UsingRepository<IUseCase<CreateVehicleInput>>(useCase =>
                useCase.Execute(new CreateVehicleInput(vehicleId, "Model Return", DateOnly.FromDateTime(DateTime.UtcNow.Date.AddYears(-1)))));

            await Fixture.UsingRepository<IUseCase<RentVehicleInput>>(useCase =>
                useCase.Execute(new RentVehicleInput(customerId, vehicleId)));

            // Assert precondition: rented vehicle is not in available list.
            var availableAfterRent = await GetAvailableVehicleIds();
            availableAfterRent.Should().NotContain(vehicleId);

            // Act
            await Fixture.UsingRepository<IUseCase<ReturnVehicleInput>>(useCase =>
                useCase.Execute(new ReturnVehicleInput(customerId, vehicleId)));

            // Assert
            var availableAfterReturn = await GetAvailableVehicleIds();
            availableAfterReturn.Should().Contain(vehicleId);
        }

        private async Task<string[]> GetAvailableVehicleIds()
        {
            string[] vehicleIds = null;

            await Fixture.UsingScope(async serviceProvider =>
            {
                var useCase = serviceProvider.GetRequiredService<IUseCase<ListAvailableVehiclesInput>>();
                var presenter = serviceProvider.GetRequiredService<ListAvailableVehiclesPresenter>();

                await useCase.Execute(new ListAvailableVehiclesInput());

                var result = Assert.IsType<OkObjectResult>(presenter.ActionResult);
                var output = Assert.IsType<ListAvailableVehiclesOutput>(result.Value);
#pragma warning disable IDE0305
                vehicleIds = output.Vehicles.Select(x => x.VehicleId).ToArray();
#pragma warning restore IDE0305
            });

            return vehicleIds!;
        }
    }
}
