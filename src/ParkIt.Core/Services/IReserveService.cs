using ParkIt.Core.Entities;

namespace ParkIt.Core.Services;

public interface IReserveService
{
    void ReserveParking(IEnumerable<ParkingSpot> parkingSpots, Reservation reservation, ParkingSpot parkingSpot);
}