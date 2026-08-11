namespace GtMotive.Estimate.Microservice.Api.UseCases
{
    public sealed class RentVehicleRequest
    {
        public string CustomerId { get; set; } = string.Empty;

        public string VehicleId { get; set; } = string.Empty;
    }
}
