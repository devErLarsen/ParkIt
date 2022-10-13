namespace ParkIt.Core.Exceptions;

public sealed class ParkingSpotAlreadyReservedForPeriodException : ParkItException
{
    public ParkingSpotAlreadyReservedForPeriodException() : base("Overlapping time periods for same parking spot")
    {
    }
}