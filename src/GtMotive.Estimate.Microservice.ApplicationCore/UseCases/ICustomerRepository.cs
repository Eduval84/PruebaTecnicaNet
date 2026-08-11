using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.Domain;

namespace GtMotive.Estimate.Microservice.ApplicationCore.UseCases
{
    /// <summary>
    /// Customer repository port.
    /// </summary>
    public interface ICustomerRepository
    {
        /// <summary>
        /// Gets a customer by id.
        /// </summary>
        /// <param name="customerId">Customer identifier.</param>
        /// <returns>The customer when found, otherwise null.</returns>
        Task<Customer> GetById(string customerId);

        /// <summary>
        /// Updates a customer aggregate.
        /// </summary>
        /// <param name="customer">Customer aggregate.</param>
        /// <returns>A task that completes when the update operation is finished.</returns>
        Task Update(Customer customer);
    }
}
