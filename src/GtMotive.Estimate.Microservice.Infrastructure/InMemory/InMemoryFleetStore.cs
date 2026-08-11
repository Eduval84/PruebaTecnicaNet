using System.Collections.Concurrent;
using GtMotive.Estimate.Microservice.Domain;

namespace GtMotive.Estimate.Microservice.Infrastructure.InMemory
{
    internal sealed class InMemoryFleetStore
    {
        public ConcurrentDictionary<string, Vehicle> Vehicles { get; } = new();

        public ConcurrentDictionary<string, Customer> Customers { get; } = new();
    }
}
