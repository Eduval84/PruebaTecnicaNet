using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.Api.UseCases;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases;
using GtMotive.Estimate.Microservice.InfrastructureTests.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace GtMotive.Estimate.Microservice.InfrastructureTests.Specs
{
    public sealed class ListAvailableVehiclesInfrastructureTests
    {
        [Fact]
        public async Task ListAvailableShouldReturnOkAndPayloadWhenUseCasePublishesVehicles()
        {
            // Arrange
            var spyState = new SpyListAvailableState();

            var hostBuilder = new WebHostBuilder()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseEnvironment("IntegrationTest")
                .UseDefaultServiceProvider(options => { options.ValidateScopes = true; })
                .ConfigureAppConfiguration((context, builder) => { builder.AddEnvironmentVariables(); })
                .UseStartup<Startup>()
                .ConfigureTestServices(services =>
                {
                    services.AddSingleton(spyState);
                    services.AddScoped<IUseCase<ListAvailableVehiclesInput>, SpyListAvailableVehiclesUseCase>();
                });

            using var server = new TestServer(hostBuilder);
            using var client = server.CreateClient();

            // Act
            var response = await client.GetAsync(new Uri("/api/vehicles/available", UriKind.Relative));
            var responseBody = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(1, spyState.ExecutionCount);
            Assert.Contains("\"vehicles\"", responseBody, StringComparison.OrdinalIgnoreCase);
            Assert.Contains("\"vehicleId\":\"vehicle-list-infra-1\"", responseBody, StringComparison.OrdinalIgnoreCase);
        }

        private sealed class SpyListAvailableState
        {
            public int ExecutionCount { get; set; }
        }

        private sealed class SpyListAvailableVehiclesUseCase(ListAvailableVehiclesPresenter presenter, SpyListAvailableState state)
            : IUseCase<ListAvailableVehiclesInput>
        {
            private readonly ListAvailableVehiclesPresenter presenter = presenter ?? throw new ArgumentNullException(nameof(presenter));
            private readonly SpyListAvailableState state = state ?? throw new ArgumentNullException(nameof(state));

            public Task Execute(ListAvailableVehiclesInput input)
            {
                ArgumentNullException.ThrowIfNull(input);
                state.ExecutionCount++;

                presenter.StandardHandle(
                    new ListAvailableVehiclesOutput(
                        [
                            new("vehicle-list-infra-1", "Model List Infra", new DateOnly(2025, 1, 10)),
                        ]));

                return Task.CompletedTask;
            }
        }
    }
}
