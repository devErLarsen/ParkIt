namespace ParkIt.Infrastructure;

public class JwtOptions
{
    public string SigningKey { get; set; }
    public string Issuer { get; set; }
    public string Audience { get; set; }
    public TimeSpan AccessExpires { get; set; }
    public TimeSpan RefreshExpires { get; set; }
}