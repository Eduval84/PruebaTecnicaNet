using System;
using System.Linq;
using System.Threading.Tasks;

namespace GtMotive.Estimate.Microservice.ApplicationCore.UseCases
{
    /// <summary>
    /// Lists available vehicles from the fleet.
    /// </summary>
    public sealed class ListAvailableVehiclesUseCase : IUseCase<ListAvailableVehiclesInput>
    {
        private readonly IVehicleRepository vehicleRepository;
        private readonly IListAvailableVehiclesOutputPort outputPort;

        /// <summary>
        /// Initializes a new instance of the <see cref="ListAvailableVehiclesUseCase"/> class.
        /// </summary>
        /// <param name="vehicleRepository">Vehicle repository.</param>
        /// <param name="outputPort">Output port.</param>
        public ListAvailableVehiclesUseCase(IVehicleRepository vehicleRepository, IListAvailableVehiclesOutputPort outputPort)
        {
            this.vehicleRepository = vehicleRepository ?? throw new ArgumentNullException(nameof(vehicleRepository));
            this.outputPort = outputPort ?? throw new ArgumentNullException(nameof(outputPort));
        }

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
