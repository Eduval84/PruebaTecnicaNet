namespace GtMotive.Estimate.Microservice.ApplicationCore.UseCases
{
    /// <summary>
    /// Input for renting a vehicle.
    /// </summary>
    public sealed class RentVehicleInput : IUseCaseInput
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RentVehicleInput"/> class.
        /// </summary>
        /// <param name="customerId">Customer identifier.</param>
        /// <param name="vehicleId">Vehicle identifier.</param>
        public RentVehicleInput(string customerId, string vehicleId)
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
