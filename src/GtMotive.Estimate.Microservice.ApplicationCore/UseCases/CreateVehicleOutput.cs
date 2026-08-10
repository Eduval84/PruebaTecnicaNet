using System;

namespace GtMotive.Estimate.Microservice.ApplicationCore.UseCases
{
    /// <summary>
    /// Output for the create vehicle use case.
    /// </summary>
    public sealed class CreateVehicleOutput : IUseCaseOutput
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateVehicleOutput"/> class.
        /// </summary>
        /// <param name="vehicleId">Vehicle identifier.</param>
        /// <param name="model">Vehicle model.</param>
        /// <param name="manufacturingDate">Manufacturing date.</param>
        public CreateVehicleOutput(string vehicleId, string model, DateOnly manufacturingDate)
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
