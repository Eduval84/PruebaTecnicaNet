using System.ComponentModel.DataAnnotations;

namespace GtMotive.Estimate.Microservice.Api.UseCases
{
    public sealed class ReturnVehicleRequest
    {
        [Required]
        public string CustomerId { get; set; } = string.Empty;

        [Required]
        public string VehicleId { get; set; } = string.Empty;
    }
}
