using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
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
    public sealed class RentalNotFoundInfrastructureTests
    {
        [Fact]
        public async Task RentShouldReturnNotFoundWhenCustomerDoesNotExist()
        {
            // Arrange
            var spyState = new SpyState();

            var hostBuilder = new WebHostBuilder()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseEnvironment("IntegrationTest")
                .UseDefaultServiceProvider(options => { options.ValidateScopes = true; })
                .ConfigureAppConfiguration((context, builder) => { builder.AddEnvironmentVariables(); })
                .UseStartup<Startup>()
                .ConfigureTestServices(services =>
                {
                    services.AddSingleton(spyState);
                    services.AddScoped<IUseCase<RentVehicleInput>, SpyRentNotFoundUseCase>();
                });

            using var server = new TestServer(hostBuilder);
            using var client = server.CreateClient();
            client.DefaultRequestHeaders.Add(Startup.TestAuthenticationHeaderName, "true");

            const string requestBody = "{\"customerId\":\"customer-404\",\"vehicleId\":\"vehicle-1\"}";
            using var content = new StringContent(requestBody, Encoding.UTF8, "application/json");

            // Act
            var response = await client.PostAsync(new Uri("/api/rentals/rent", UriKind.Relative), content);
            var responseBody = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            Assert.Equal(1, spyState.ExecutionCount);
            Assert.Contains("customer-404", responseBody, StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public async Task ReturnShouldReturnNotFoundWhenVehicleDoesNotExist()
        {
            // Arrange
            var spyState = new SpyState();

            var hostBuilder = new WebHostBuilder()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseEnvironment("IntegrationTest")
                .UseDefaultServiceProvider(options => { options.ValidateScopes = true; })
                .ConfigureAppConfiguration((context, builder) => { builder.AddEnvironmentVariables(); })
                .UseStartup<Startup>()
                .ConfigureTestServices(services =>
                {
                    services.AddSingleton(spyState);
                    services.AddScoped<IUseCase<ReturnVehicleInput>, SpyReturnNotFoundUseCase>();
                });

            using var server = new TestServer(hostBuilder);
            using var client = server.CreateClient();
            client.DefaultRequestHeaders.Add(Startup.TestAuthenticationHeaderName, "true");

            const string requestBody = "{\"customerId\":\"customer-1\",\"vehicleId\":\"vehicle-404\"}";
            using var content = new StringContent(requestBody, Encoding.UTF8, "application/json");

            // Act
            var response = await client.PostAsync(new Uri("/api/rentals/return", UriKind.Relative), content);
            var responseBody = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            Assert.Equal(1, spyState.ExecutionCount);
            Assert.Contains("vehicle-404", responseBody, StringComparison.OrdinalIgnoreCase);
        }

        private sealed class SpyState
        {
            public int ExecutionCount { get; set; }
        }

        private sealed class SpyRentNotFoundUseCase(RentVehiclePresenter presenter, SpyState state)
            : IUseCase<RentVehicleInput>
        {
            private readonly RentVehiclePresenter presenter = presenter ?? throw new ArgumentNullException(nameof(presenter));
            private readonly SpyState state = state ?? throw new ArgumentNullException(nameof(state));

            public Task Execute(RentVehicleInput input)
            {
                ArgumentNullException.ThrowIfNull(input);
                state.ExecutionCount++;

                presenter.NotFoundHandle($"Customer '{input.CustomerId}' was not found.");

                return Task.CompletedTask;
            }
        }

        private sealed class SpyReturnNotFoundUseCase(ReturnVehiclePresenter presenter, SpyState state)
            : IUseCase<ReturnVehicleInput>
        {
            private readonly ReturnVehiclePresenter presenter = presenter ?? throw new ArgumentNullException(nameof(presenter));
            private readonly SpyState state = state ?? throw new ArgumentNullException(nameof(state));

            public Task Execute(ReturnVehicleInput input)
            {
                ArgumentNullException.ThrowIfNull(input);
                state.ExecutionCount++;

                presenter.NotFoundHandle($"Vehicle '{input.VehicleId}' was not found for return.");

                return Task.CompletedTask;
            }
        }
    }
}
