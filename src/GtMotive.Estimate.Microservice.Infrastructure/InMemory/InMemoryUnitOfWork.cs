using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.Domain.Interfaces;

namespace GtMotive.Estimate.Microservice.Infrastructure.InMemory
{
    internal sealed class InMemoryUnitOfWork : IUnitOfWork
    {
        public Task<int> Save()
        {
            return Task.FromResult(1);
        }
    }
}
