namespace FlightApi.Dto.Response;

public class AirportResponse
{
    public Guid Id { get; set; }
    public string IataCode { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? City { get; set; }
    public string? Country { get; set; }
}
