using ParkIt.Core.Exceptions;

namespace ParkIt.Application.Exceptions;

public sealed class ParkingSpotNotFoundException : ParkItException
{
    public ParkingSpotNotFoundException() : base("No Parking Spot found") { }
    public ParkingSpotNotFoundException(string value) : base($"No Parking spot found for given Id: {value}.")
    {
    }
}
