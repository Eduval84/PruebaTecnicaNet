using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.InfrastructureTests.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace GtMotive.Estimate.Microservice.InfrastructureTests.Specs
{
    public sealed class AuthenticatedEndpointsInfrastructureTests
    {
        [Fact]
        public async Task CreateVehicleShouldRejectAnonymousRequests()
        {
            // Arrange
            var hostBuilder = new WebHostBuilder()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseEnvironment("IntegrationTest")
                .UseDefaultServiceProvider(options => { options.ValidateScopes = true; })
                .ConfigureAppConfiguration((context, builder) => { builder.AddEnvironmentVariables(); })
                .UseStartup<Startup>();

            using var server = new TestServer(hostBuilder);
            using var client = server.CreateClient();
            using var content = new StringContent(
                "{\"vehicleId\":\"vehicle-anon-1\",\"model\":\"Model Anonymous\",\"manufacturingDate\":\"2025-01-15\"}",
                Encoding.UTF8,
                "application/json");

            // Act
            var response = await client.PostAsync(new Uri("/api/vehicles/create", UriKind.Relative), content);

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task ListAvailableShouldRejectAnonymousRequests()
        {
            // Arrange
            var hostBuilder = new WebHostBuilder()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseEnvironment("IntegrationTest")
                .UseDefaultServiceProvider(options => { options.ValidateScopes = true; })
                .ConfigureAppConfiguration((context, builder) => { builder.AddEnvironmentVariables(); })
                .UseStartup<Startup>();

            using var server = new TestServer(hostBuilder);
            using var client = server.CreateClient();

            // Act
            var response = await client.GetAsync(new Uri("/api/vehicles/available", UriKind.Relative));

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }
    }
}
