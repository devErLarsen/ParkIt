using Microsoft.AspNetCore.Identity;

namespace ParkIt.Infrastructure.Auth;

public class ParkItUser : IdentityUser
{
    public Guid EmployeeId { get; set; }
    public string RefreshToken { get; set; }
    public DateTime RefreshTokenExpires { get; set; }
}