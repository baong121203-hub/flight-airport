using FlightApi.Dto.Request;
using FlightApi.Dto.Response;

namespace FlightApi.Service;

public interface IFlightService
{
    Task<List<FlightResponse>> GetAllAsync();
    Task<FlightResponse?> GetByIdAsync(Guid id);
    Task<FlightResponse> CreateAsync(CreateFlightRequest request);
    Task<FlightResponse?> UpdateAsync(Guid id, UpdateFlightRequest request);
    Task<bool> DeleteAsync(Guid id);
}
