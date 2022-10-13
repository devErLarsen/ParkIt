using ParkIt.Core.Enums;
using ParkIt.Core.Exceptions;

namespace ParkIt.Core.Entities;

public class CarSpot : ParkingSpot
{
    public CarSpot(Guid id, string spotCode) : base(id, spotCode)
    {
    }

    public override void ReserveParkingSpot(Reservation reservation)
    {
        var vehicle = reservation.Employee.Vehicles
            .FirstOrDefault(v => v.NumberPlate == reservation.NumberPlate);
        if (vehicle == null)
            throw new VehicleNotFoundException();
        if (vehicle.Type != VehicleType.Car)
            throw new ParkingSpotReservationException("Parking spot is exclusively for cars");
        if (_reservations.Any(x =>
            reservation.ReservationPeriod.Start < x.ReservationPeriod.End
            && x.ReservationPeriod.Start < reservation.ReservationPeriod.End))
        {
            throw new ParkingSpotAlreadyReservedForPeriodException();
        }
        
        _reservations.Add(reservation);
    }
}