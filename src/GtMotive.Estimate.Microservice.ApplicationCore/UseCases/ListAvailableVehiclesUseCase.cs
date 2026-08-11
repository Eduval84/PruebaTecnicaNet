using System;
using System.Linq;
using System.Threading.Tasks;

namespace GtMotive.Estimate.Microservice.ApplicationCore.UseCases
{
    /// <summary>
    /// Lists available vehicles from the fleet.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="ListAvailableVehiclesUseCase"/> class.
    /// </remarks>
    /// <param name="vehicleRepository">Vehicle repository.</param>
    /// <param name="outputPort">Output port.</param>
    public sealed class ListAvailableVehiclesUseCase(IVehicleRepository vehicleRepository, IListAvailableVehiclesOutputPort outputPort) : IUseCase<ListAvailableVehiclesInput>
    {
        private readonly IVehicleRepository vehicleRepository = vehicleRepository ?? throw new ArgumentNullException(nameof(vehicleRepository));
        private readonly IListAvailableVehiclesOutputPort outputPort = outputPort ?? throw new ArgumentNullException(nameof(outputPort));

        /// <inheritdoc />
        public async Task Execute(ListAvailableVehiclesInput input)
        {
            ArgumentNullException.ThrowIfNull(input);

            var vehicles = await vehicleRepository.ListAvailable();
            var outputItems = vehicles
                .Select(x => new ListAvailableVehiclesOutputItem(x.Id, x.Model, x.ManufacturingDate.Value))
                .ToList();

            outputPort.StandardHandle(new ListAvailableVehiclesOutput(outputItems));
        }
    }
}
