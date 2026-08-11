using System;

namespace GtMotive.Estimate.Microservice.Domain
{
    /// <summary>
    /// Represents a rentable vehicle in the fleet.
    /// </summary>
    public sealed class Vehicle
    {
        private bool isRented;

        private Vehicle(string id, string model, ManufacturingDate manufacturingDate)
        {
            Id = id;
            Model = model;
            ManufacturingDate = manufacturingDate;
        }

        /// <summary>
        /// Gets the vehicle identifier.
        /// </summary>
        public string Id { get; }

        /// <summary>
        /// Gets the vehicle model.
        /// </summary>
        public string Model { get; }

        /// <summary>
        /// Gets the validated manufacturing date.
        /// </summary>
        public ManufacturingDate ManufacturingDate { get; }

        /// <summary>
        /// Gets a value indicating whether the vehicle is available.
        /// </summary>
        public bool IsAvailable => !isRented;

        /// <summary>
        /// Creates a vehicle aggregate.
        /// </summary>
        /// <param name="id">Vehicle identifier.</param>
        /// <param name="model">Vehicle model.</param>
        /// <param name="manufacturingDate">Validated manufacturing date.</param>
        /// <returns>A new <see cref="Vehicle"/> instance.</returns>
        public static Vehicle Create(string id, string model, ManufacturingDate manufacturingDate)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new DomainException("Vehicle id is required.");
            }

            if (string.IsNullOrWhiteSpace(model))
            {
                throw new DomainException("Vehicle model is required.");
            }

            ArgumentNullException.ThrowIfNull(manufacturingDate);

            return new Vehicle(id, model, manufacturingDate);
        }

        /// <summary>
        /// Marks the vehicle as rented.
        /// </summary>
        public void Rent()
        {
            if (isRented)
            {
                throw new DomainException("Vehicle is already rented.");
            }

            isRented = true;
        }

        /// <summary>
        /// Marks the vehicle as returned.
        /// </summary>
        public void Return()
        {
            if (!isRented)
            {
                throw new DomainException("Vehicle is not currently rented.");
            }

            isRented = false;
        }
    }
}
