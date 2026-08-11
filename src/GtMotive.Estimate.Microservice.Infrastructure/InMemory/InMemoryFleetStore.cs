using System.Collections.Concurrent;
using GtMotive.Estimate.Microservice.Domain;

namespace GtMotive.Estimate.Microservice.Infrastructure.InMemory
{
    internal sealed class InMemoryFleetStore
    {
        public InMemoryFleetStore()
        {
            Customers["CUS-001"] = Customer.Create("CUS-001");
        }

        public ConcurrentDictionary<string, Vehicle> Vehicles { get; } = new();

        public ConcurrentDictionary<string, Customer> Customers { get; } = new();
    }
}
