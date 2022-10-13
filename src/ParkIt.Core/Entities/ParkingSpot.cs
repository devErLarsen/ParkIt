using System.Text.RegularExpressions;
using ParkIt.Core.Exceptions;

namespace ParkIt.Core.Entities;

public abstract class ParkingSpot
{
    public Guid Id { get; private set; }
    public string SpotCode { get; private set; }
    // public int SpotCapacity { get; private set; }
    // public bool HandicapSpot { get; private set; }
    
    //Navigation property
    public IEnumerable<Reservation> Reservations => _reservations;

    protected readonly HashSet<Reservation> _reservations = new();
    
    // valid SpotName
    private static readonly Regex _spotNamePattern = new("[A-Z]{1}\\d{1,3}", RegexOptions.Compiled);
    // private const int MaxParkingCapacity = 2;

    private ParkingSpot()
    {
    }

    public ParkingSpot(Guid id, string spotCode) //, bool handicapSpot = false)
    {
        Id = id;
        if (!_spotNamePattern.IsMatch(spotCode))
            throw new InvalidSpotNameException(spotCode);
        SpotCode = spotCode;
        // SpotCapacity = MaxParkingCapacity;
        
        // return new ParkingSpot
        // {
        //     Id = id,
        //     SpotCode = spotCode,
        //     SpotCapacity = MaxParkingCapacity,
        //     // HandicapSpot = handicapSpot
        // };
    }

    public abstract void ReserveParkingSpot(Reservation reservation);
    // public void ReserveParkingSpot(Reservation reservation)
    // {
    //     var overlappingReservation = _reservations.Where(x =>
    //         reservation.ReservationPeriod.Start < x.ReservationPeriod.End
    //         && x.ReservationPeriod.Start < reservation.ReservationPeriod.End).ToList();
    //
    //     if (overlappingReservation.Any())
    //     {
    //         var capCount = overlappingReservation.Select(res => res.Employee.Vehicles
    //             .SingleOrDefault(x => x.NumberPlate == res.NumberPlate))
    //             .Where(vehicle => vehicle != null).Sum(vehicle => (int)vehicle.Type);
    //         if (capCount > SpotCapacity)
    //         {
    //             throw new ParkingSpotReservationException("Reservation Exceeds parking spot capacity limit.");
    //         }
    //     }
    //     // if (_reservations.Any(x =>
    //     //     reservation.ReservationPeriod.Start < x.ReservationPeriod.End
    //     //     && x.ReservationPeriod.Start < reservation.ReservationPeriod.End))
    //     // {
    //     //     throw new ParkingSpotAlreadyReservedForPeriodException();
    //     // }
    //     
    //     _reservations.Add(reservation);
    // }

    public void UpdateReservation(Reservation reservation)
    {
        RemoveReservation(reservation.Id);
        ReserveParkingSpot(reservation);
    }

    public void RemoveReservation(Guid reservationId)
        => _reservations.RemoveWhere(x => x.Id == reservationId);

    // public void ChangeHandicapStatus(bool value) => HandicapSpot = value;
}