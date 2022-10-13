using Microsoft.EntityFrameworkCore;
using ParkIt.Core.Entities;
using ParkIt.Core.Repositories;

namespace ParkIt.Infrastructure.DAL.Repositories;

internal sealed class ParkingSpotRepository : IParkingSpotRepository
{
    private readonly ParkItDbContext _context;
    private readonly DbSet<ParkingSpot> _parkingSpots;

    public ParkingSpotRepository(ParkItDbContext context)
    {
        _context = context;
        _parkingSpots = _context.ParkingSpots;
    }

    public async Task<IEnumerable<ParkingSpot>> GetAllAsync()
        => await _parkingSpots
            .Include(x => x.Reservations)
            .ThenInclude(e => e.Employee)
            .ThenInclude(v => v.Vehicles).ToListAsync();
        // => await _parkingSpots.Include(x => x.Reservations).ThenInclude(r => r.Employee).ToListAsync();
        // => await _parkingSpots.Include(x => x.Reservations).ToListAsync();

        public async Task<ParkingSpot> GetAsync(Guid parkingSpotId)
            => await _parkingSpots
                .Include(x => x.Reservations)
                .ThenInclude(e => e.Employee)
                .ThenInclude(v => v.Vehicles)
                .SingleOrDefaultAsync(x => x.Id == parkingSpotId);
        // => await _parkingSpots.Include(x => x.Reservations).ThenInclude(r => r.Employee).SingleOrDefaultAsync(x => x.Id == parkingSpotId);
        // => await _parkingSpots.Include(x => x.Reservations).SingleOrDefaultAsync(x => x.Id == parkingSpotId);

    public async Task AddAsync(ParkingSpot parkingSpot)
    {
        _parkingSpots.Add(parkingSpot);
        await _context.SaveChangesAsync();
    }

    public async Task AddRangeAsync(IEnumerable<ParkingSpot> parkingSpots)
    {
        _parkingSpots.AddRange(parkingSpots);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(ParkingSpot parkingSpot)
    {
        _parkingSpots.Update(parkingSpot);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(ParkingSpot parkingSpot)
    {
        _parkingSpots.Remove(parkingSpot);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteRangeAsync(ParkingSpot[] parkingSpots)
    {
        _parkingSpots.RemoveRange(parkingSpots);
        await _context.SaveChangesAsync();
    }
}
