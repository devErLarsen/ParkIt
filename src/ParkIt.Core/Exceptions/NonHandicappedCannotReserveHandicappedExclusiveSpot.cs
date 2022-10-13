namespace ParkIt.Core.Exceptions;

public class NonHandicappedCannotReserveHandicappedExclusiveSpot : ParkItException
{
    public NonHandicappedCannotReserveHandicappedExclusiveSpot() : base(
        "Parking spot marked for handicapped only. Employee attempting to reserve this spot must have 'Handicapped' set to true.")
    {
    }
}