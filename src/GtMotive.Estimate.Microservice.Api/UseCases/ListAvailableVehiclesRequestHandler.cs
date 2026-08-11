using System;
using System.Threading;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases;
using MediatR;

namespace GtMotive.Estimate.Microservice.Api.UseCases
{
    public sealed class ListAvailableVehiclesRequestHandler(IUseCase<ListAvailableVehiclesInput> useCase, ListAvailableVehiclesPresenter presenter) : IRequestHandler<ListAvailableVehiclesRequest, IWebApiPresenter>
    {
        private readonly IUseCase<ListAvailableVehiclesInput> useCase = useCase ?? throw new ArgumentNullException(nameof(useCase));
        private readonly ListAvailableVehiclesPresenter presenter = presenter ?? throw new ArgumentNullException(nameof(presenter));

        public async Task<IWebApiPresenter> Handle(ListAvailableVehiclesRequest request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);

            await useCase.Execute(new ListAvailableVehiclesInput());
            return presenter;
        }
    }
}
