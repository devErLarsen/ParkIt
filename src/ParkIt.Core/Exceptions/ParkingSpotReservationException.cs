namespace ParkIt.Core.Exceptions;

public class ParkingSpotReservationException : ParkItException
{
    public ParkingSpotReservationException(string message) : base(message)
    {
    }
}