using System;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.Domain.Interfaces;

namespace GtMotive.Estimate.Microservice.ApplicationCore.UseCases
{
    /// <summary>
    /// Returns a rented vehicle.
    /// </summary>
    public sealed class ReturnVehicleUseCase : IUseCase<ReturnVehicleInput>
    {
        private readonly ICustomerRepository customerRepository;
        private readonly IVehicleRepository vehicleRepository;
        private readonly IUnitOfWork unitOfWork;
        private readonly IReturnVehicleOutputPort outputPort;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReturnVehicleUseCase"/> class.
        /// </summary>
        /// <param name="customerRepository">Customer repository.</param>
        /// <param name="vehicleRepository">Vehicle repository.</param>
        /// <param name="unitOfWork">Unit of work.</param>
        /// <param name="outputPort">Output port.</param>
        public ReturnVehicleUseCase(ICustomerRepository customerRepository, IVehicleRepository vehicleRepository, IUnitOfWork unitOfWork, IReturnVehicleOutputPort outputPort)
        {
            this.customerRepository = customerRepository ?? throw new ArgumentNullException(nameof(customerRepository));
            this.vehicleRepository = vehicleRepository ?? throw new ArgumentNullException(nameof(vehicleRepository));
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            this.outputPort = outputPort ?? throw new ArgumentNullException(nameof(outputPort));
        }

        /// <inheritdoc />
        public async Task Execute(ReturnVehicleInput input)
        {
            ArgumentNullException.ThrowIfNull(input);

            var customer = await customerRepository.GetById(input.CustomerId);
            var vehicle = await vehicleRepository.GetById(input.VehicleId);

            if (customer is null)
            {
                outputPort.NotFoundHandle($"Customer '{input.CustomerId}' was not found.");
                return;
            }

            if (vehicle is null)
            {
                outputPort.NotFoundHandle($"Vehicle '{input.VehicleId}' was not found.");
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
