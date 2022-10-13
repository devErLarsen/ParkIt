using ParkIt.Core.Exceptions;

namespace ParkIt.Application.Exceptions;

public sealed class UserNotFoundException : ParkItException
{
    public UserNotFoundException(string id) : base($"No user found with the id: {id}.")
    {
    }
}