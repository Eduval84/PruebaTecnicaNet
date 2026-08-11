using System;
using System.Collections.Generic;

namespace GtMotive.Estimate.Microservice.ApplicationCore.UseCases
{
    /// <summary>
    /// Output for listing available vehicles.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="ListAvailableVehiclesOutput"/> class.
    /// </remarks>
    /// <param name="vehicles">Available vehicles.</param>
    public sealed class ListAvailableVehiclesOutput(IReadOnlyCollection<ListAvailableVehiclesOutputItem> vehicles) : IUseCaseOutput
    {

        /// <summary>
        /// Gets the available vehicles.
        /// </summary>
        public IReadOnlyCollection<ListAvailableVehiclesOutputItem> Vehicles { get; } = vehicles ?? throw new ArgumentNullException(nameof(vehicles));
    }
}
