using System;

namespace GtMotive.Estimate.Microservice.Domain
{
    /// <summary>
    /// Represents a vehicle manufacturing date constrained by domain rules.
    /// </summary>
    public sealed class ManufacturingDate
    {
        private ManufacturingDate(DateOnly value)
        {
            Value = value;
        }

        /// <summary>
        /// Gets the validated manufacturing date value.
        /// </summary>
        public DateOnly Value { get; }

        /// <summary>
        /// Creates a validated manufacturing date.
        /// </summary>
        /// <param name="value">Manufacturing date to validate.</param>
        /// <returns>A validated <see cref="ManufacturingDate"/> instance.</returns>
        /// <exception cref="DomainException">Thrown when the date is older than five years.</exception>
        public static ManufacturingDate Create(DateOnly value)
        {
            var oldestAllowedDate = DateOnly.FromDateTime(DateTime.UtcNow.Date.AddYears(-5));

            if (value < oldestAllowedDate)
            {
                throw new DomainException("Manufacturing date cannot be older than five years.");
            }

            return new ManufacturingDate(value);
        }
    }
}
