using ParkIt.Core.Enums;
using ParkIt.Core.Exceptions;

namespace ParkIt.Core.Entities;

public class BikeSpot : ParkingSpot
{
    public int SpotCapacity { get; private set; }
    private const int MaxSpotCapacity = 2;
    public BikeSpot(Guid id, string spotCode) : base(id, spotCode)
    {
        SpotCapacity = MaxSpotCapacity;
    }

    public override void ReserveParkingSpot(Reservation reservation)
    {
        var vehicle = reservation.Employee.Vehicles
            .FirstOrDefault(v => v.NumberPlate == reservation.NumberPlate);
        if (vehicle == null)
            throw new VehicleNotFoundException();
        if (vehicle.Type != VehicleType.Bike)
            throw new ParkingSpotReservationException("Parking spot is exclusively for bikes");
        var overlappingReservation = _reservations.Where(x =>
            reservation.ReservationPeriod.Start < x.ReservationPeriod.End
            && x.ReservationPeriod.Start < reservation.ReservationPeriod.End).ToList();
        
        if (overlappingReservation.Any())
        {
            var capCount = overlappingReservation.Select(res => res.Employee.Vehicles
                .SingleOrDefault(x => x.NumberPlate == res.NumberPlate))
                .Where(v => v != null).Sum(v => (int)v.Type);
            if (capCount > SpotCapacity)
            {
                throw new ParkingSpotReservationException("Reservation Exceeds parking spot capacity limit.");
            }
        }

        _reservations.Add(reservation);

    }
}