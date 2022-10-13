namespace ParkIt.Core.Exceptions;

public sealed class InvalidReservationPeriodException : ParkItException
{
    public InvalidReservationPeriodException() : base("Parking spot reservation limited to between 7am and 6pm")
    {
    }
}