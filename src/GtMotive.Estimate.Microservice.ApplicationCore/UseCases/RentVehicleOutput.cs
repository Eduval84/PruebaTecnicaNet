namespace GtMotive.Estimate.Microservice.ApplicationCore.UseCases
{
    /// <summary>
    /// Output for renting a vehicle.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="RentVehicleOutput"/> class.
    /// </remarks>
    /// <param name="customerId">Customer identifier.</param>
    /// <param name="vehicleId">Vehicle identifier.</param>
    public sealed class RentVehicleOutput(string customerId, string vehicleId) : IUseCaseOutput
    {

        /// <summary>
        /// Gets the customer identifier.
        /// </summary>
        public string CustomerId { get; } = customerId;

        /// <summary>
        /// Gets the vehicle identifier.
        /// </summary>
        public string VehicleId { get; } = vehicleId;
    }
}
