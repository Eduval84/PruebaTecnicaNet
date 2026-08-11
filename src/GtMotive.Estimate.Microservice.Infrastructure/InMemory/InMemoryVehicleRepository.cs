using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases;
using GtMotive.Estimate.Microservice.Domain;

namespace GtMotive.Estimate.Microservice.Infrastructure.InMemory
{
    internal sealed class InMemoryVehicleRepository(InMemoryFleetStore fleetStore) : IVehicleRepository
    {
        private readonly InMemoryFleetStore fleetStore = fleetStore ?? throw new ArgumentNullException(nameof(fleetStore));

        public Task Add(Vehicle vehicle)
        {
            ArgumentNullException.ThrowIfNull(vehicle);

            fleetStore.Vehicles[vehicle.Id] = vehicle;
            return Task.CompletedTask;
        }

        public Task<Vehicle> GetById(string vehicleId)
        {
            fleetStore.Vehicles.TryGetValue(vehicleId, out var vehicle);
            return Task.FromResult(vehicle);
        }

        public Task Update(Vehicle vehicle)
        {
            ArgumentNullException.ThrowIfNull(vehicle);

            if (!fleetStore.Vehicles.ContainsKey(vehicle.Id))
            {
                throw new DomainException($"Vehicle '{vehicle.Id}' was not found for update.");
            }

            fleetStore.Vehicles[vehicle.Id] = vehicle;
            return Task.CompletedTask;
        }

        public Task<IReadOnlyCollection<Vehicle>> ListAvailable()
        {
            var availableVehicles = fleetStore.Vehicles.Values
                .Where(x => x.IsAvailable)
                .ToArray();

            return Task.FromResult<IReadOnlyCollection<Vehicle>>(availableVehicles);
        }
    }
}
