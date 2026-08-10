using System;
using FluentAssertions;
using GtMotive.Estimate.Microservice.Domain;
using Xunit;

namespace GtMotive.Estimate.Microservice.UnitTests;

public class VehicleManufacturingDateTests
{
    [Fact]
    public void Create_ShouldThrowDomainException_WhenDateIsOlderThanFiveYears()
    {
        // Arrange
        var invalidManufacturingDate = DateOnly.FromDateTime(DateTime.UtcNow.Date.AddYears(-6));

        // Act
        Action act = () => ManufacturingDate.Create(invalidManufacturingDate);

        // Assert
        act.Should().Throw<DomainException>();
    }
}
