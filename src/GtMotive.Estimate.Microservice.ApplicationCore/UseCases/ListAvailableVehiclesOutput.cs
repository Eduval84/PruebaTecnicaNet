using System;
using System.Collections.Generic;

namespace GtMotive.Estimate.Microservice.ApplicationCore.UseCases
{
    /// <summary>
    /// Output for listing available vehicles.
    /// </summary>
    public sealed class ListAvailableVehiclesOutput : IUseCaseOutput
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ListAvailableVehiclesOutput"/> class.
        /// </summary>
        /// <param name="vehicles">Available vehicles.</param>
        public ListAvailableVehiclesOutput(IReadOnlyCollection<ListAvailableVehiclesOutputItem> vehicles)
        {
            Vehicles = vehicles ?? throw new ArgumentNullException(nameof(vehicles));
        }

        /// <summary>
        /// Gets the available vehicles.
        /// </summary>
        public IReadOnlyCollection<ListAvailableVehiclesOutputItem> Vehicles { get; }
    }
}
