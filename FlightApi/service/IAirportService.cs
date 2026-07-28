using FlightApi.Dto.Request;
using FlightApi.Dto.Response;

namespace FlightApi.Service;

public interface IAirportService
{
    Task<List<AirportResponse>> GetAllAsync();
    Task<AirportResponse?> GetByIdAsync(Guid id);
    Task<AirportResponse> CreateAsync(CreateAirportRequest request);
    Task<AirportResponse?> UpdateAsync(Guid id, UpdateAirportRequest request);
    Task<bool> DeleteAsync(Guid id);
}
