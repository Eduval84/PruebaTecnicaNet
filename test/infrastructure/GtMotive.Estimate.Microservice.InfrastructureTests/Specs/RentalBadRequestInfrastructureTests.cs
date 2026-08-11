using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases;
using GtMotive.Estimate.Microservice.InfrastructureTests.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace GtMotive.Estimate.Microservice.InfrastructureTests.Specs
{
    public sealed class RentalBadRequestInfrastructureTests
    {
        [Fact]
        public async Task RentShouldReturnBadRequestAndNotInvokeUseCaseWhenCustomerIdIsMissing()
        {
            // Arrange
            var spyUseCase = new SpyRentVehicleUseCase();

            var hostBuilder = new WebHostBuilder()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseEnvironment("IntegrationTest")
                .UseDefaultServiceProvider(options => { options.ValidateScopes = true; })
                .ConfigureAppConfiguration((context, builder) => { builder.AddEnvironmentVariables(); })
                .UseStartup<Startup>()
                .ConfigureTestServices(services =>
                {
                    services.AddSingleton(spyUseCase);
                    services.AddSingleton<IUseCase<RentVehicleInput>>(sp => sp.GetRequiredService<SpyRentVehicleUseCase>());
                });

            using var server = new TestServer(hostBuilder);
            using var client = server.CreateClient();
            client.DefaultRequestHeaders.Add(Startup.TestAuthenticationHeaderName, "true");

            const string invalidRequestBody = "{\"vehicleId\":\"vehicle-rent-bad-request\"}";
            using var content = new StringContent(invalidRequestBody, Encoding.UTF8, "application/json");

            // Act
            var response = await client.PostAsync(new Uri("/api/rentals/rent", UriKind.Relative), content);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Equal(0, spyUseCase.ExecutionCount);
            Assert.Null(spyUseCase.LastInput);
        }

        [Fact]
        public async Task ReturnShouldReturnBadRequestAndNotInvokeUseCaseWhenVehicleIdIsMissing()
        {
            // Arrange
            var spyUseCase = new SpyReturnVehicleUseCase();

            var hostBuilder = new WebHostBuilder()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseEnvironment("IntegrationTest")
                .UseDefaultServiceProvider(options => { options.ValidateScopes = true; })
                .ConfigureAppConfiguration((context, builder) => { builder.AddEnvironmentVariables(); })
                .UseStartup<Startup>()
                .ConfigureTestServices(services =>
                {
                    services.AddSingleton(spyUseCase);
                    services.AddSingleton<IUseCase<ReturnVehicleInput>>(sp => sp.GetRequiredService<SpyReturnVehicleUseCase>());
                });

            using var server = new TestServer(hostBuilder);
            using var client = server.CreateClient();
            client.DefaultRequestHeaders.Add(Startup.TestAuthenticationHeaderName, "true");

            const string invalidRequestBody = "{\"customerId\":\"customer-return-bad-request\"}";
            using var content = new StringContent(invalidRequestBody, Encoding.UTF8, "application/json");

            // Act
            var response = await client.PostAsync(new Uri("/api/rentals/return", UriKind.Relative), content);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Equal(0, spyUseCase.ExecutionCount);
            Assert.Null(spyUseCase.LastInput);
        }

        private sealed class SpyRentVehicleUseCase : IUseCase<RentVehicleInput>
        {
            public int ExecutionCount { get; private set; }

            public RentVehicleInput LastInput { get; private set; }

            public Task Execute(RentVehicleInput input)
            {
                ArgumentNullException.ThrowIfNull(input);
                ExecutionCount++;
                LastInput = input;
                return Task.CompletedTask;
            }
        }

        private sealed class SpyReturnVehicleUseCase : IUseCase<ReturnVehicleInput>
        {
            public int ExecutionCount { get; private set; }

            public ReturnVehicleInput LastInput { get; private set; }

            public Task Execute(ReturnVehicleInput input)
            {
                ArgumentNullException.ThrowIfNull(input);
                ExecutionCount++;
                LastInput = input;
                return Task.CompletedTask;
            }
        }
    }
}
