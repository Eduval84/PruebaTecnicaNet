namespace GtMotive.Estimate.Microservice.ApplicationCore.UseCases
{
    /// <summary>
    /// Input for returning a vehicle.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="ReturnVehicleInput"/> class.
    /// </remarks>
    /// <param name="customerId">Customer identifier.</param>
    /// <param name="vehicleId">Vehicle identifier.</param>
    public sealed class ReturnVehicleInput(string customerId, string vehicleId) : IUseCaseInput
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
