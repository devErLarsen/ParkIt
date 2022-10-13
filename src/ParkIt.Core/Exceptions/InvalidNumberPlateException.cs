namespace ParkIt.Core.Exceptions;

public sealed class InvalidNumberPlateException : ParkItException
{
    public InvalidNumberPlateException(string numberPlate)
        : base($"Invalid plate: {numberPlate}")
    {
    }
}