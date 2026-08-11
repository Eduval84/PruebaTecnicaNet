using System;
using System.Text.Json.Serialization;

namespace GtMotive.Estimate.Microservice.Api.UseCases
{
    public sealed class CreateVehicleRequest
    {
        public string VehicleId { get; set; } = string.Empty;

        public string Model { get; set; } = string.Empty;

        [JsonRequired]
        public DateOnly ManufacturingDate { get; set; }
    }
}
