using System;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.Domain.Interfaces;

namespace GtMotive.Estimate.Microservice.ApplicationCore.UseCases
{
    /// <summary>
    /// Rents a vehicle for a customer.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="RentVehicleUseCase"/> class.
    /// </remarks>
    /// <param name="customerRepository">Customer repository.</param>
    /// <param name="vehicleRepository">Vehicle repository.</param>
    /// <param name="unitOfWork">Unit of work.</param>
    /// <param name="outputPort">Output port.</param>
    public sealed class RentVehicleUseCase(ICustomerRepository customerRepository, IVehicleRepository vehicleRepository, IUnitOfWork unitOfWork, IRentVehicleOutputPort outputPort) : IUseCase<RentVehicleInput>
    {
        private readonly ICustomerRepository customerRepository = customerRepository ?? throw new ArgumentNullException(nameof(customerRepository));
        private readonly IVehicleRepository vehicleRepository = vehicleRepository ?? throw new ArgumentNullException(nameof(vehicleRepository));
        private readonly IUnitOfWork unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        private readonly IRentVehicleOutputPort outputPort = outputPort ?? throw new ArgumentNullException(nameof(outputPort));

        /// <inheritdoc />
        public async Task Execute(RentVehicleInput input)
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
                outputPort.NotFoundHandle($"Vehicle '{input.VehicleId}' was not found for rent.");
                return;
            }

            if (!vehicle.IsAvailable)
            {
                outputPort.NotFoundHandle($"Vehicle '{input.VehicleId}' is not available for rent.");
                return;
            }

            customer.StartRental(vehicle.Id);
            vehicle.Rent();

            await customerRepository.Update(customer);
            await vehicleRepository.Update(vehicle);
            await unitOfWork.Save();

            outputPort.StandardHandle(new RentVehicleOutput(customer.Id, vehicle.Id));
        }
    }
}
