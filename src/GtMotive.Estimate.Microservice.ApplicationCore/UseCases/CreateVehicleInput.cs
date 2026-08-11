using System;

namespace GtMotive.Estimate.Microservice.ApplicationCore.UseCases
{
    /// <summary>
    /// Input for the create vehicle use case.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="CreateVehicleInput"/> class.
    /// </remarks>
    /// <param name="vehicleId">Vehicle identifier.</param>
    /// <param name="model">Vehicle model.</param>
    /// <param name="manufacturingDate">Manufacturing date.</param>
    public sealed class CreateVehicleInput(string vehicleId, string model, DateOnly manufacturingDate) : IUseCaseInput
    {

        /// <summary>
        /// Gets the vehicle identifier.
        /// </summary>
        public string VehicleId { get; } = vehicleId;

        /// <summary>
        /// Gets the vehicle model.
        /// </summary>
        public string Model { get; } = model;

        /// <summary>
        /// Gets the manufacturing date.
        /// </summary>
        public DateOnly ManufacturingDate { get; } = manufacturingDate;
    }
}
