using ParkIt.Core.Entities;
using ParkIt.Core.Exceptions;

namespace ParkIt.Core.Services;

public class ReserveService : IReserveService
{
    public void ReserveParking(IEnumerable<ParkingSpot> parkingSpots, Reservation reservation, ParkingSpot parkingSpot)
    {
        var allReservations = parkingSpots
            .SelectMany(x => x.Reservations)
            .Where(x => x.ReservationPeriod.Start.Date == reservation.ReservationPeriod.Start.Date);
        if (allReservations.Any(x => x.NumberPlate == reservation.NumberPlate))
        {
            throw new ParkingSpotReservationException("Cannot Reserve more than one parking spot at a time.");
        }
        
        

        parkingSpot.ReserveParkingSpot(reservation);
    }
}