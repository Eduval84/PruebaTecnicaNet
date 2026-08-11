namespace GtMotive.Estimate.Microservice.Api.UseCases
{
    public sealed class ReturnVehicleRequest
    {
        public string CustomerId { get; set; } = string.Empty;

        public string VehicleId { get; set; } = string.Empty;
    }
}
