using System;
using System.Text.Json.Serialization;
using MediatR;

namespace GtMotive.Estimate.Microservice.Api.UseCases
{
    public sealed class CreateVehicleRequest : IRequest<IWebApiPresenter>
    {
        public string VehicleId { get; set; } = string.Empty;

        public string Model { get; set; } = string.Empty;

        [JsonRequired]
        public DateOnly ManufacturingDate { get; set; }
    }
}
