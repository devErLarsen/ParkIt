namespace ParkIt.Core.Exceptions;

public abstract class ParkItException : Exception
{
    protected ParkItException(string message) : base(message)
    {
    }
}