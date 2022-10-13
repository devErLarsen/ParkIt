namespace ParkIt.Core.Exceptions;

public sealed class InvalidEmailException : ParkItException
{
    public InvalidEmailException(string email) : base($"Invalid Email provided: {email}.")
    {
    }
}
