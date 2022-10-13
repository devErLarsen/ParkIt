using ParkIt.Core.Exceptions;

namespace ParkIt.Application.Exceptions;
public sealed class IncorrectPasswordException : ParkItException
{
    public IncorrectPasswordException() : base("Incorrect password provided.")
    {
    }
}