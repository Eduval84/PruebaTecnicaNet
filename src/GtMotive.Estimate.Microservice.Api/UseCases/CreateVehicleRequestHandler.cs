using System;
using System.Threading;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases;
using MediatR;

namespace GtMotive.Estimate.Microservice.Api.UseCases
{
    public sealed class CreateVehicleRequestHandler(IUseCase<CreateVehicleInput> useCase, CreateVehiclePresenter presenter) : IRequestHandler<CreateVehicleRequest, IWebApiPresenter>
    {
        private readonly IUseCase<CreateVehicleInput> useCase = useCase ?? throw new ArgumentNullException(nameof(useCase));
        private readonly CreateVehiclePresenter presenter = presenter ?? throw new ArgumentNullException(nameof(presenter));

        public async Task<IWebApiPresenter> Handle(CreateVehicleRequest request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);

            await useCase.Execute(new CreateVehicleInput(request.VehicleId, request.Model, request.ManufacturingDate));
            return presenter;
        }
    }
}
