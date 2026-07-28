using FlightApi.Data;
using FlightApi.Model;
using Microsoft.EntityFrameworkCore;

namespace FlightApi.Repository;

public class FlightRepository : IFlightRepository
{
    private readonly ApplicationDbContext _context;

    public FlightRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Flight>> GetAllAsync()
    {
        return await _context.Flights
            .Include(f => f.OriginAirport)
            .Include(f => f.DestinationAirport)
            .OrderBy(f => f.FlightDate)
            .ThenBy(f => f.ScheduledTime)
            .ToListAsync();
    }

    public async Task<Flight?> GetByIdAsync(Guid id)
    {
        return await _context.Flights
            .Include(f => f.OriginAirport)
            .Include(f => f.DestinationAirport)
            .FirstOrDefaultAsync(f => f.Id == id);
    }

    public async Task<Flight> AddAsync(Flight flight)
    {
        _context.Flights.Add(flight);
        await _context.SaveChangesAsync();

        return (await GetByIdAsync(flight.Id))!;
    }

    public async Task UpdateAsync(Flight flight)
    {
        _context.Entry(flight).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Flight flight)
    {
        _context.Flights.Remove(flight);
        await _context.SaveChangesAsync();
    }
}
