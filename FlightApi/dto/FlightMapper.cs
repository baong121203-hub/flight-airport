using FlightApi.Dto.Request;
using FlightApi.Dto.Response;
using FlightApi.Model;

namespace FlightApi.Dto;

public static class FlightMapper
{
    public static Flight ToEntity(CreateFlightRequest request)
    {
        return new Flight
        {
            Id = Guid.NewGuid(),
            FlightNo = request.FlightNo.ToUpperInvariant(),
            FlightDate = request.FlightDate,
            ArrDep = request.ArrDep.ToUpperInvariant(),
            Status = request.Status.ToUpperInvariant(),
            OriginAirportId = request.OriginAirportId,
            DestinationAirportId = request.DestinationAirportId,
            AircraftReg = request.AircraftReg,
            AircraftType = request.AircraftType.ToUpperInvariant(),
            FlightType = request.FlightType.ToUpperInvariant(),
            NatureOfFlight = string.IsNullOrWhiteSpace(request.NatureOfFlight) ? "---" : request.NatureOfFlight,
            ScheduledTime = request.ScheduledTime,
            EstimatedTime = request.EstimatedTime,
            ActualTime = request.ActualTime,
            ParkingStand = request.ParkingStand,
            Gate = request.Gate,
            BookingPax = request.BookingPax,
            CreatedAt = DateTime.UtcNow
        };
    }

    public static void ApplyUpdate(Flight flight, UpdateFlightRequest request)
    {
        flight.FlightNo = request.FlightNo.ToUpperInvariant();
        flight.FlightDate = request.FlightDate;
        flight.ArrDep = request.ArrDep.ToUpperInvariant();
        flight.Status = request.Status.ToUpperInvariant();
        flight.OriginAirportId = request.OriginAirportId;
        flight.DestinationAirportId = request.DestinationAirportId;
        flight.AircraftReg = request.AircraftReg;
        flight.AircraftType = request.AircraftType.ToUpperInvariant();
        flight.FlightType = request.FlightType.ToUpperInvariant();
        flight.NatureOfFlight = string.IsNullOrWhiteSpace(request.NatureOfFlight) ? "---" : request.NatureOfFlight;
        flight.ScheduledTime = request.ScheduledTime;
        flight.EstimatedTime = request.EstimatedTime;
        flight.ActualTime = request.ActualTime;
        flight.ParkingStand = request.ParkingStand;
        flight.Gate = request.Gate;
        flight.BookingPax = request.BookingPax;
        flight.UpdatedAt = DateTime.UtcNow;
    }

    public static FlightResponse ToResponse(Flight flight)
    {
        return new FlightResponse
        {
            Id = flight.Id,
            FlightNo = flight.FlightNo,
            FlightDate = flight.FlightDate,
            ArrDep = flight.ArrDep,
            Status = flight.Status,
            OriginAirportId = flight.OriginAirportId,
            OriginIata = flight.OriginAirport?.IataCode ?? string.Empty,
            DestinationAirportId = flight.DestinationAirportId,
            DestinationIata = flight.DestinationAirport?.IataCode ?? string.Empty,
            AircraftReg = flight.AircraftReg,
            AircraftType = flight.AircraftType,
            FlightType = flight.FlightType,
            NatureOfFlight = flight.NatureOfFlight,
            ScheduledTime = flight.ScheduledTime,
            EstimatedTime = flight.EstimatedTime,
            ActualTime = flight.ActualTime,
            ParkingStand = flight.ParkingStand,
            Gate = flight.Gate,
            BookingPax = flight.BookingPax,
            CreatedAt = flight.CreatedAt,
            UpdatedAt = flight.UpdatedAt
        };
    }

    public static List<FlightResponse> ToResponseList(IEnumerable<Flight> flights)
    {
        return flights.Select(ToResponse).ToList();
    }
}
