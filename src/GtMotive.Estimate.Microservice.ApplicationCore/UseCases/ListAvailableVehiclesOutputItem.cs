using System;

namespace GtMotive.Estimate.Microservice.ApplicationCore.UseCases
{
    /// <summary>
    /// Represents a single vehicle in the list available output.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="ListAvailableVehiclesOutputItem"/> class.
    /// </remarks>
    /// <param name="vehicleId">Vehicle identifier.</param>
    /// <param name="model">Vehicle model.</param>
    /// <param name="manufacturingDate">Manufacturing date.</param>
    public sealed class ListAvailableVehiclesOutputItem(string vehicleId, string model, DateOnly manufacturingDate)
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
