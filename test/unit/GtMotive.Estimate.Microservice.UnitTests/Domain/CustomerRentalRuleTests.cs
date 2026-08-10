using System;
using FluentAssertions;
using GtMotive.Estimate.Microservice.Domain;
using Xunit;

namespace GtMotive.Estimate.Microservice.UnitTests.Domain
{
    public class CustomerRentalRuleTests
    {
        [Fact]
        public void StartRentalShouldThrowDomainExceptionWhenCustomerAlreadyHasActiveRental()
        {
            // Arrange
            var customer = Customer.Create("customer-1");

            customer.StartRental("vehicle-1");

            // Act
            Action act = () => customer.StartRental("vehicle-2");

            // Assert
            act.Should().Throw<DomainException>();
        }
    }
}
