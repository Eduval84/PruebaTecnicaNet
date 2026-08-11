using System;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases;
using GtMotive.Estimate.Microservice.Domain;

namespace GtMotive.Estimate.Microservice.Infrastructure.InMemory
{
    internal sealed class InMemoryCustomerRepository(InMemoryFleetStore fleetStore) : ICustomerRepository
    {
        private readonly InMemoryFleetStore fleetStore = fleetStore ?? throw new ArgumentNullException(nameof(fleetStore));

        public Task<Customer> GetById(string customerId)
        {
            fleetStore.Customers.TryGetValue(customerId, out var customer);
            return Task.FromResult(customer);
        }

        public Task Update(Customer customer)
        {
            ArgumentNullException.ThrowIfNull(customer);

            fleetStore.Customers[customer.Id] = customer;
            return Task.CompletedTask;
        }
    }
}
