using System;

namespace GtMotive.Estimate.Microservice.ApplicationCore.UseCases
{
    /// <summary>
    /// Represents a single vehicle in the list available output.
    /// </summary>
    public sealed class ListAvailableVehiclesOutputItem
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ListAvailableVehiclesOutputItem"/> class.
        /// </summary>
        /// <param name="vehicleId">Vehicle identifier.</param>
        /// <param name="model">Vehicle model.</param>
        /// <param name="manufacturingDate">Manufacturing date.</param>
        public ListAvailableVehiclesOutputItem(string vehicleId, string model, DateOnly manufacturingDate)
        {
            VehicleId = vehicleId;
            Model = model;
            ManufacturingDate = manufacturingDate;
        }

        /// <summary>
        /// Gets the vehicle identifier.
        /// </summary>
        public string VehicleId { get; }

        /// <summary>
        /// Gets the vehicle model.
        /// </summary>
        public string Model { get; }

        /// <summary>
        /// Gets the manufacturing date.
        /// </summary>
        public DateOnly ManufacturingDate { get; }
    }
}
