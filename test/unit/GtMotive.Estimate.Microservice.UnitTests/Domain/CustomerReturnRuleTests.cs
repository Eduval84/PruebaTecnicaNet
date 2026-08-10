using System;
using FluentAssertions;
using GtMotive.Estimate.Microservice.Domain;
using Xunit;

namespace GtMotive.Estimate.Microservice.UnitTests.Domain
{
    public class CustomerReturnRuleTests
    {
        [Fact]
        public void EndRentalShouldAllowStartingNewRentalAfterReturn()
        {
            // Arrange
            var customer = Customer.Create("customer-1");
            customer.StartRental("vehicle-1");

            // Act
            customer.EndRental();

            Action act = () => customer.StartRental("vehicle-2");

            // Assert
            act.Should().NotThrow();
        }
    }
}
