namespace ParkIt.Infrastructure.Auth;

public record RefreshToken(string Token, DateTime Expires);