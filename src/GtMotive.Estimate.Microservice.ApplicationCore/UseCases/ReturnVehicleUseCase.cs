using System;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.Domain.Interfaces;

namespace GtMotive.Estimate.Microservice.ApplicationCore.UseCases
{
    /// <summary>
    /// Returns a rented vehicle.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="ReturnVehicleUseCase"/> class.
    /// </remarks>
    /// <param name="customerRepository">Customer repository.</param>
    /// <param name="vehicleRepository">Vehicle repository.</param>
    /// <param name="unitOfWork">Unit of work.</param>
    /// <param name="outputPort">Output port.</param>
    public sealed class ReturnVehicleUseCase(ICustomerRepository customerRepository, IVehicleRepository vehicleRepository, IUnitOfWork unitOfWork, IReturnVehicleOutputPort outputPort) : IUseCase<ReturnVehicleInput>
    {
        private readonly ICustomerRepository customerRepository = customerRepository ?? throw new ArgumentNullException(nameof(customerRepository));
        private readonly IVehicleRepository vehicleRepository = vehicleRepository ?? throw new ArgumentNullException(nameof(vehicleRepository));
        private readonly IUnitOfWork unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        private readonly IReturnVehicleOutputPort outputPort = outputPort ?? throw new ArgumentNullException(nameof(outputPort));

        /// <inheritdoc />
        public async Task Execute(ReturnVehicleInput input)
        {
            ArgumentNullException.ThrowIfNull(input);

            var customer = await customerRepository.GetById(input.CustomerId);

            if (customer is null)
            {
                outputPort.NotFoundHandle($"Customer '{input.CustomerId}' was not found.");
                return;
            }

            var vehicle = await vehicleRepository.GetById(input.VehicleId);

            if (vehicle is null)
            {
                outputPort.NotFoundHandle($"Vehicle '{input.VehicleId}' was not found for return.");
                return;
            }

            if (vehicle.IsAvailable)
            {
                outputPort.NotFoundHandle($"Vehicle '{input.VehicleId}' does not have an active rental.");
                return;
            }

            customer.EndRental();
            vehicle.Return();

            await customerRepository.Update(customer);
            await vehicleRepository.Update(vehicle);
            await unitOfWork.Save();

            outputPort.StandardHandle(new ReturnVehicleOutput(customer.Id, vehicle.Id));
        }
    }
}
