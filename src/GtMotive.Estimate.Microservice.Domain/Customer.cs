namespace GtMotive.Estimate.Microservice.Domain
{
    /// <summary>
    /// Represents a customer that can rent at most one vehicle at a time.
    /// </summary>
    public sealed class Customer
    {
        private bool hasActiveRental;

        private Customer(string id)
        {
            Id = id;
        }

        /// <summary>
        /// Gets the customer identifier.
        /// </summary>
        public string Id { get; }

        /// <summary>
        /// Creates a customer aggregate instance.
        /// </summary>
        /// <param name="id">Customer identifier.</param>
        /// <returns>A new <see cref="Customer"/> instance.</returns>
        /// <exception cref="DomainException">Thrown when id is null, empty, or whitespace.</exception>
        public static Customer Create(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new DomainException("Customer id is required.");
            }

            return new Customer(id);
        }

        /// <summary>
        /// Starts a rental for the customer.
        /// </summary>
        /// <param name="vehicleId">Vehicle identifier.</param>
        /// <exception cref="DomainException">Thrown when the customer already has an active rental.</exception>
        public void StartRental(string vehicleId)
        {
            if (string.IsNullOrWhiteSpace(vehicleId))
            {
                throw new DomainException("Vehicle id is required.");
            }

            if (hasActiveRental)
            {
                throw new DomainException("A customer cannot have more than one active rental.");
            }

            hasActiveRental = true;
        }
    }
}
