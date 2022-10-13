using ParkIt.Core.Exceptions;

namespace ParkIt.Application.Exceptions;
public sealed class UserDoesNotExistException : ParkItException
{
    public UserDoesNotExistException() : base("User does not exist.")
    {
    }
}