using FlightApi.Data;
using FlightApi.Model;
using Microsoft.EntityFrameworkCore;

namespace FlightApi.Repository;

public class AirportRepository : IAirportRepository
{
    private readonly ApplicationDbContext _context;

    public AirportRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Airport>> GetAllAsync()
    {
        return await _context.Airports
            .OrderBy(a => a.IataCode)
            .ToListAsync();
    }

    public async Task<Airport?> GetByIdAsync(Guid id)
    {
        return await _context.Airports.FindAsync(id);
    }

    public async Task<Airport> AddAsync(Airport airport)
    {
        _context.Airports.Add(airport);
        await _context.SaveChangesAsync();
        return airport;
    }

    public async Task UpdateAsync(Airport airport)
    {
        _context.Entry(airport).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Airport airport)
    {
        _context.Airports.Remove(airport);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> ExistsByIataCodeAsync(string iataCode, Guid? excludeId = null)
    {
        var query = _context.Airports.Where(a => a.IataCode == iataCode);
        if (excludeId.HasValue)
        {
            query = query.Where(a => a.Id != excludeId.Value);
        }

        return await query.AnyAsync();
    }
}
