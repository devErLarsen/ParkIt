using ParkIt.Core.Exceptions;

namespace ParkIt.Core.Entities;

public class CarSpot : ParkingSpot
{
    public CarSpot(Guid id, string spotCode) : base(id, spotCode)
    {
    }

    public override void ReserveParkingSpot(Reservation reservation)
    {
        if (_reservations.Any(x =>
            reservation.ReservationPeriod.Start < x.ReservationPeriod.End
            && x.ReservationPeriod.Start < reservation.ReservationPeriod.End))
        {
            throw new ParkingSpotAlreadyReservedForPeriodException();
        }
        
        _reservations.Add(reservation);
    }
}