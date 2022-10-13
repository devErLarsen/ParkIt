using Microsoft.AspNetCore.Identity;
// using ParkIt.Application.Contracts;
using ParkIt.Application.DTOs;
using ParkIt.Application.Exceptions;
using ParkIt.Application.Services;
using ParkIt.Contracts.Request;
using ParkIt.Contracts.Response;
using ParkIt.Core.Entities;
using ParkIt.Core.Repositories;

namespace ParkIt.Infrastructure.Auth;

public class UserService : IUserService
{
    private readonly UserManager<ParkItUser> _userManager;
    private readonly IEmployeeRepository _employees;
    private readonly ITokenHandler _tokenHandler;

    public UserService(IEmployeeRepository employees,
        UserManager<ParkItUser> userManager,
        ITokenHandler tokenHandler)
    {
        _employees = employees;
        _userManager = userManager;
        _tokenHandler = tokenHandler;
    }

    public async Task<Tokens> Login(LoginRequest request)
    {
        var parkItUser = await _userManager.FindByNameAsync(request.Email); // name = email
        if (parkItUser is null)
            throw new UserDoesNotExistException();


        var checkPassword = await _userManager.CheckPasswordAsync(parkItUser, request.Password);
        if (!checkPassword)
            throw new IncorrectPasswordException();

        // var employee = await _employees.GetByEmailAsync(request.Email);

        var roles = await _userManager.GetRolesAsync(parkItUser);
        var token = _tokenHandler.CreateToken(parkItUser, roles.First());
        // var token = _tokenHandler.CreateToken(claims);

        var refreshToken = _tokenHandler.CreateRefreshToken();

        parkItUser.RefreshToken = refreshToken.Token;
        parkItUser.RefreshTokenExpires = refreshToken.Expires;

        await _userManager.UpdateAsync(parkItUser);

        return new Tokens
        {
            AccessToken = token,
            RefreshToken = refreshToken.Token,
            RefreshExpires = refreshToken.Expires
        };
    }

    public async Task<Tokens> Register(RegisterRequest request)
    {
        // var (name, email, password, role) = request;
        var role = string.IsNullOrEmpty(request.Role) ? "User" : null;
        var existingUser = await _userManager.FindByNameAsync(request.Email); // name = email
        if (existingUser is not null)
        {
            throw new UserAlreadyExistsException();
        }

        var refreshToken = _tokenHandler.CreateRefreshToken();

        var employeeId = Guid.NewGuid();

        var newUser = new ParkItUser
        {
            UserName = request.Email,
            EmployeeId = employeeId,
            RefreshToken = refreshToken.Token,
            RefreshTokenExpires = refreshToken.Expires
        };

        var createdUser = await _userManager.CreateAsync(newUser, request.Password);
        if (!createdUser.Succeeded)
            throw new Exception();

        await _userManager.AddToRoleAsync(newUser, role);

        var newEmployee = new Employee(employeeId, request.Name, request.Email, DateTime.Now); //Enum.Parse<Role>(role)
        await _employees.AddAsync(newEmployee);

        var token = _tokenHandler.CreateToken(newUser, role);

        return new Tokens
        {
            AccessToken = token,
            RefreshToken = refreshToken.Token,
            RefreshExpires = refreshToken.Expires
        };

    }
    
    public async Task Logout(string identityId)
    {
        var user = await _userManager.FindByIdAsync(identityId);
        user.RefreshToken = default;
        user.RefreshTokenExpires = default;
        await _userManager.UpdateAsync(user);
    }

    public async Task<Tokens> RefreshToken(string accessToken, string refreshToken)
    {
        var claimsFromToken = _tokenHandler.ParseExpiredToken(accessToken);
        var idClaim = claimsFromToken.First(x => x.Type == "IdentityId").Value;
        // var id = JsonSerializer.Deserialize<string>(idClaim.ToString());
        var parkItUser = await _userManager.FindByIdAsync(idClaim);
        if (!parkItUser.RefreshToken.Equals(refreshToken))
        {
            return new Tokens();
        }
        else if (parkItUser.RefreshTokenExpires < DateTime.Now)
        {
            return new Tokens();
        }

        var roles = await _userManager.GetRolesAsync(parkItUser);
        var newAccessToken = _tokenHandler.CreateToken(parkItUser, roles.First());

        var newRefreshToken = _tokenHandler.CreateRefreshToken();

        parkItUser.RefreshToken = newRefreshToken.Token;
        parkItUser.RefreshTokenExpires = newRefreshToken.Expires;

        await _userManager.UpdateAsync(parkItUser);

        return new Tokens
        {
            AccessToken = newAccessToken,
            RefreshToken = newRefreshToken.Token,
            RefreshExpires = newRefreshToken.Expires
        };
    }

    // public async Task<UserResponse> GetCurrentUser(string id)
    // {
    //     var parkItUser = await _userManager.FindByIdAsync(id);
    //     return new UserResponse(parkItUser.UserName);
    // }
}