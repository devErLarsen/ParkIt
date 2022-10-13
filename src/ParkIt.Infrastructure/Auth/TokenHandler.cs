using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ParkIt.Application.Auth;

namespace ParkIt.Infrastructure.Auth;

public class TokenHandler : ITokenHandler
{
    private readonly JwtOptions _jwtOptions;

    public TokenHandler(IOptions<JwtOptions> options)
    {
        _jwtOptions = options.Value;
    }

    public string CreateToken(ParkItUser user, string role = "User") //(string id, Guid employeeId, string name, string email, string role)
    {
        var key = _jwtOptions.SigningKey;
        var secret = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        var signingCredentials = new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            // new(JwtRegisteredClaimNames.Sub, user.Id),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            // new(JwtRegisteredClaimNames.Email, user.UserName),
            // new(JwtRegisteredClaimNames.UniqueName, user.UserName),
            new("EmployeeId", user.EmployeeId.ToString()),
            new("IdentityId", user.Id),
            new(ClaimTypes.Name, user.UserName),
            new(ClaimTypes.Role, role)
        };

        var expires = DateTime.Now.Add(_jwtOptions.AccessExpires);
        var jwt = new JwtSecurityToken(_jwtOptions.Issuer, _jwtOptions.Audience, claims, DateTime.Now, expires, signingCredentials);
        var token = new JwtSecurityTokenHandler().WriteToken(jwt);

        return token;
    }

    public RefreshToken CreateRefreshToken()
        => new(Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)), DateTime.Now.Add(_jwtOptions.RefreshExpires));

    public List<Claim> ParseExpiredToken(string token)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false,
            ValidateIssuer = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SigningKey)),
            ValidateLifetime = false
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
        if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            throw new SecurityTokenException("Invalid token");

        return principal.Claims.ToList();
    }
}