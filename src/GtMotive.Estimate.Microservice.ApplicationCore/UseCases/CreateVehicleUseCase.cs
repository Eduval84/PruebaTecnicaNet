using System;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.Domain;
using GtMotive.Estimate.Microservice.Domain.Interfaces;

namespace GtMotive.Estimate.Microservice.ApplicationCore.UseCases
{
    /// <summary>
    /// Creates a new vehicle in the fleet.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="CreateVehicleUseCase"/> class.
    /// </remarks>
    /// <param name="vehicleRepository">Vehicle repository.</param>
    /// <param name="unitOfWork">Unit of work.</param>
    /// <param name="outputPort">Output port.</param>
    public sealed class CreateVehicleUseCase(IVehicleRepository vehicleRepository, IUnitOfWork unitOfWork, ICreateVehicleOutputPort outputPort) : IUseCase<CreateVehicleInput>
    {
        private readonly IVehicleRepository vehicleRepository = vehicleRepository ?? throw new ArgumentNullException(nameof(vehicleRepository));
        private readonly IUnitOfWork unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        private readonly ICreateVehicleOutputPort outputPort = outputPort ?? throw new ArgumentNullException(nameof(outputPort));

        /// <inheritdoc />
        public async Task Execute(CreateVehicleInput input)
        {
            ArgumentNullException.ThrowIfNull(input);

            var manufacturingDate = ManufacturingDate.Create(input.ManufacturingDate);
            var vehicle = Vehicle.Create(input.VehicleId, input.Model, manufacturingDate);

            await vehicleRepository.Add(vehicle);
            await unitOfWork.Save();

            outputPort.StandardHandle(new CreateVehicleOutput(vehicle.Id, vehicle.Model, vehicle.ManufacturingDate.Value));
        }
    }
}
