namespace ParkIt.Core.Exceptions;

public class VehicleNotFoundException : ParkItException
{
    public VehicleNotFoundException() : base("Vehicle not found!")
    {
    }
}