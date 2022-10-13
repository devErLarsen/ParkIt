namespace ParkIt.Core.Exceptions;

public sealed class InvalidSpotNameException : ParkItException
{
    public InvalidSpotNameException(string name) : base($"Invalid parking spot name provided: {name}, Combine 'A-Z' with '1-100'.")
    { }
}