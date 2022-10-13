using ParkIt.Core.Exceptions;

namespace ParkIt.Application.Exceptions;
public sealed class UserAlreadyExistsException : ParkItException
{
    public UserAlreadyExistsException() : base("Email already exists in the system")
    {
    }
}