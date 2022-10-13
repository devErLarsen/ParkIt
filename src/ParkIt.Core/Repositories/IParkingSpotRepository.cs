using ParkIt.Core.Entities;

namespace ParkIt.Core.Repositories;

public interface IParkingSpotRepository
{
    Task<IEnumerable<ParkingSpot>> GetAllAsync();
    Task<ParkingSpot> GetAsync(Guid parkingSpotId);
    Task AddAsync(ParkingSpot parkingSpot);
    Task AddRangeAsync(IEnumerable<ParkingSpot> parkingSpots);
    Task UpdateAsync(ParkingSpot parkingSpot);
    Task DeleteAsync(ParkingSpot parkingSpot);
    Task DeleteRangeAsync(ParkingSpot[] parkingSpots);
}