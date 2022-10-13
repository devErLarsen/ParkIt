using ParkIt.Core.ValueObjects;

namespace ParkIt.Core.Entities;

public class Reservation
{
    public Guid Id { get; private set; }
    // public Guid ParkingSpotId { get; private set; }
    public Guid EmployeeId { get; private set; }
    public NumberPlate NumberPlate { get; private set; }
    public ReservationPeriod ReservationPeriod { get; private set; }

    // Navigation prop
    // public Vehicle Vehicle { get; private set; }
    public Employee Employee { get; private set; }
    // public ParkingSpot ParkingSpot{ get; private set; }

    // ef core
    private Reservation()
    {
    }
    public Reservation(Guid id, Employee employee, NumberPlate numberPlate, ReservationPeriod reservationPeriod)
    {
        Id = id;
        Employee = employee;
        NumberPlate = numberPlate;
        ReservationPeriod = reservationPeriod;
    }
    

    public void ChangeNumberPlate(NumberPlate newNumberPlate)
        => NumberPlate = newNumberPlate;

}