using ParkIt.Contracts.Request;
using ParkIt.Contracts.Response;

namespace ParkIt.Application.Auth;

public interface IAuthHandler
{
    // Task<TokenResponse> CreateToken();
    Task<TokenResponse> Login(LoginRequest request);
    Task<TokenResponse> Register(RegisterRequest request);
    Task<EmployeeResponse> GetCurrent(Guid id);
    Task<IEnumerable<EmployeeResponse>> GetAll();
    Task UpdateEmployee(Guid id, UpdateEmployeeRequest request);
}