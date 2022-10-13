using ParkIt.Core.Exceptions;

namespace ParkIt.Core.Entities;

public class HandicapSpot : CarSpot
{
    public HandicapSpot(Guid id, string spotCode) : base(id, spotCode)
    {
    }

    public override void ReserveParkingSpot(Reservation reservation)
    {
        if (reservation.Employee.HandicapPrivilege)
            base.ReserveParkingSpot(reservation);
        else
            throw new ParkingSpotReservationException(
                "Employee not eligible to reserve parking spot for disabled people");
    }
}