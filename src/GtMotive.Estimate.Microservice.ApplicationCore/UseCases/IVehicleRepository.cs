using System.Collections.Generic;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.Domain;

namespace GtMotive.Estimate.Microservice.ApplicationCore.UseCases
{
    /// <summary>
    /// Vehicle repository port.
    /// </summary>
    public interface IVehicleRepository
    {
        /// <summary>
        /// Adds a vehicle to the fleet.
        /// </summary>
        /// <param name="vehicle">Vehicle aggregate.</param>
        /// <returns>A task that completes when the add operation is finished.</returns>
        Task Add(Vehicle vehicle);

        /// <summary>
        /// Lists available vehicles in the fleet.
        /// </summary>
        /// <returns>The available vehicles.</returns>
        Task<IReadOnlyCollection<Vehicle>> ListAvailable();
    }
}
