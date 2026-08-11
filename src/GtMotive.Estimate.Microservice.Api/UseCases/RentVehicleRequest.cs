using System.ComponentModel.DataAnnotations;
using MediatR;

namespace GtMotive.Estimate.Microservice.Api.UseCases
{
    public sealed class RentVehicleRequest : IRequest<IWebApiPresenter>
    {
        [Required]
        public string CustomerId { get; set; } = string.Empty;

        [Required]
        public string VehicleId { get; set; } = string.Empty;
    }
}
