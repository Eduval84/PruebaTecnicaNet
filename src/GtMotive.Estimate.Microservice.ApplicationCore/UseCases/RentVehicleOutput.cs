namespace GtMotive.Estimate.Microservice.ApplicationCore.UseCases
{
    /// <summary>
    /// Output for renting a vehicle.
    /// </summary>
    public sealed class RentVehicleOutput : IUseCaseOutput
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RentVehicleOutput"/> class.
        /// </summary>
        /// <param name="customerId">Customer identifier.</param>
        /// <param name="vehicleId">Vehicle identifier.</param>
        public RentVehicleOutput(string customerId, string vehicleId)
        {
            CustomerId = customerId;
            VehicleId = vehicleId;
        }

        /// <summary>
        /// Gets the customer identifier.
        /// </summary>
        public string CustomerId { get; }

        /// <summary>
        /// Gets the vehicle identifier.
        /// </summary>
        public string VehicleId { get; }
    }
}
