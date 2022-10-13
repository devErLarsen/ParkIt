using System.Security.Claims;

namespace ParkIt.Infrastructure.Auth;
public interface ITokenHandler
{
    string CreateToken(ParkItUser user, string role);
    RefreshToken CreateRefreshToken();
    List<Claim> ParseExpiredToken(string token);
}