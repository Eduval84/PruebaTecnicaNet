using System;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.Domain.Interfaces;

namespace GtMotive.Estimate.Microservice.ApplicationCore.UseCases
{
    /// <summary>
    /// Rents a vehicle for a customer.
    /// </summary>
    public sealed class RentVehicleUseCase : IUseCase<RentVehicleInput>
    {
        private readonly ICustomerRepository customerRepository;
        private readonly IVehicleRepository vehicleRepository;
        private readonly IUnitOfWork unitOfWork;
        private readonly IRentVehicleOutputPort outputPort;

        /// <summary>
        /// Initializes a new instance of the <see cref="RentVehicleUseCase"/> class.
        /// </summary>
        /// <param name="customerRepository">Customer repository.</param>
        /// <param name="vehicleRepository">Vehicle repository.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        /// <param name="outputPort">Output port.</param>
        public RentVehicleUseCase(ICustomerRepository customerRepository, IVehicleRepository vehicleRepository, IUnitOfWork unitOfWork, IRentVehicleOutputPort outputPort)
        {
            this.customerRepository = customerRepository ?? throw new ArgumentNullException(nameof(customerRepository));
            this.vehicleRepository = vehicleRepository ?? throw new ArgumentNullException(nameof(vehicleRepository));
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            this.outputPort = outputPort ?? throw new ArgumentNullException(nameof(outputPort));
        }

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
