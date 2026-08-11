using System;
using System.Threading;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases;
using MediatR;

namespace GtMotive.Estimate.Microservice.Api.UseCases
{
    public sealed class RentVehicleRequestHandler(IUseCase<RentVehicleInput> useCase, RentVehiclePresenter presenter) : IRequestHandler<RentVehicleRequest, IWebApiPresenter>
    {
        private readonly IUseCase<RentVehicleInput> useCase = useCase ?? throw new ArgumentNullException(nameof(useCase));
        private readonly RentVehiclePresenter presenter = presenter ?? throw new ArgumentNullException(nameof(presenter));

        public async Task<IWebApiPresenter> Handle(RentVehicleRequest request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);

            await useCase.Execute(new RentVehicleInput(request.CustomerId, request.VehicleId));
            return presenter;
        }
    }
}
