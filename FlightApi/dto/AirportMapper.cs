using FlightApi.Dto.Request;
using FlightApi.Dto.Response;
using FlightApi.Model;

namespace FlightApi.Dto;

public static class AirportMapper
{
    public static Airport ToEntity(CreateAirportRequest request)
    {
        return new Airport
        {
            Id = Guid.NewGuid(),
            IataCode = request.IataCode.ToUpperInvariant(),
            Name = request.Name,
            City = request.City,
            Country = request.Country ?? "Vietnam"
        };
    }

    public static void ApplyUpdate(Airport airport, UpdateAirportRequest request)
    {
        airport.IataCode = request.IataCode.ToUpperInvariant();
        airport.Name = request.Name;
        airport.City = request.City;
        airport.Country = request.Country ?? "Vietnam";
    }

    public static AirportResponse ToResponse(Airport airport)
    {
        return new AirportResponse
        {
            Id = airport.Id,
            IataCode = airport.IataCode,
            Name = airport.Name,
            City = airport.City,
            Country = airport.Country
        };
    }

    public static List<AirportResponse> ToResponseList(IEnumerable<Airport> airports)
    {
        return airports.Select(ToResponse).ToList();
    }
}
