using System;
using System.Threading;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases;
using MediatR;

namespace GtMotive.Estimate.Microservice.Api.UseCases
{
    public sealed class ReturnVehicleRequestHandler(IUseCase<ReturnVehicleInput> useCase, ReturnVehiclePresenter presenter) : IRequestHandler<ReturnVehicleRequest, IWebApiPresenter>
    {
        private readonly IUseCase<ReturnVehicleInput> useCase = useCase ?? throw new ArgumentNullException(nameof(useCase));
        private readonly ReturnVehiclePresenter presenter = presenter ?? throw new ArgumentNullException(nameof(presenter));

        public async Task<IWebApiPresenter> Handle(ReturnVehicleRequest request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);

            await useCase.Execute(new ReturnVehicleInput(request.CustomerId, request.VehicleId));
            return presenter;
        }
    }
}
