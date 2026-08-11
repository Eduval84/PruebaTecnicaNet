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
    public sealed class CreateVehicleRequestValidationInfrastructureTests
    {
        [Fact]
        public async Task CreateShouldBindPayloadAndInvokeUseCase()
        {
            // Arrange
            var spyUseCase = new SpyCreateVehicleUseCase();

            var hostBuilder = new WebHostBuilder()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseEnvironment("IntegrationTest")
                .UseDefaultServiceProvider(options => { options.ValidateScopes = true; })
                .ConfigureAppConfiguration((context, builder) => { builder.AddEnvironmentVariables(); })
                .UseStartup<Startup>()
                .ConfigureTestServices(services =>
                {
                    services.AddSingleton(spyUseCase);
                    services.AddSingleton<IUseCase<CreateVehicleInput>>(sp => sp.GetRequiredService<SpyCreateVehicleUseCase>());
                });

            using var server = new TestServer(hostBuilder);
            using var client = server.CreateClient();

            const string validRequestBody = "{\"vehicleId\":\"vehicle-infra-1\",\"model\":\"Model Infra\",\"manufacturingDate\":\"2025-01-15\"}";

            using var content = new StringContent(validRequestBody, Encoding.UTF8, "application/json");

            // Act
            var response = await client.PostAsync(new Uri("/api/vehicles/create", UriKind.Relative), content);

            // Assert
            Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
            Assert.Equal(1, spyUseCase.ExecutionCount);
            Assert.NotNull(spyUseCase.LastInput);
            Assert.Equal("vehicle-infra-1", spyUseCase.LastInput!.VehicleId);
            Assert.Equal("Model Infra", spyUseCase.LastInput.Model);
            Assert.Equal(new DateOnly(2025, 1, 15), spyUseCase.LastInput.ManufacturingDate);
        }

        private sealed class SpyCreateVehicleUseCase : IUseCase<CreateVehicleInput>
        {
            public int ExecutionCount { get; private set; }

            public CreateVehicleInput LastInput { get; private set; }

            public Task Execute(CreateVehicleInput input)
            {
                ArgumentNullException.ThrowIfNull(input);
                ExecutionCount++;
                LastInput = input;
                return Task.CompletedTask;
            }
        }
    }
}
