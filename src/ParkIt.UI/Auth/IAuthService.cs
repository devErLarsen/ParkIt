using ParkIt.Contracts.Request;
using ParkIt.Contracts.Response;

namespace ParkIt.UI.Auth;

public interface IAuthService
{
    // Task<bool> Register(RegisterRequest request);
    Task<bool> Authenticate<T>(T request ,string endpoint);
    Task Logout();
}