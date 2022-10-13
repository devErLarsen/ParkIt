namespace ParkIt.Core.Exceptions;

public sealed class InvalidNameException : ParkItException
{
    public InvalidNameException(string name) : base($"Invalid name provided: {name}, must be between 3 and 30 characters.")
    {
    }
}