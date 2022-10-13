// using ParkIt.Application.Contracts;
using ParkIt.Application.DTOs;
using ParkIt.Contracts.Request;
using ParkIt.Contracts.Response;

namespace ParkIt.Application.Services;

public interface IUserService
{
    Task<Tokens> Register(RegisterRequest request);
    Task<Tokens> Login(LoginRequest request);
    Task Logout(string identityId);
    Task<Tokens> RefreshToken(string accessToken, string refreshToken);
    // Task<UserResponse> GetCurrentUser(string id);
}