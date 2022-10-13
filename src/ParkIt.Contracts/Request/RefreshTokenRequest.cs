namespace ParkIt.Contracts.Request;

public record RefreshTokenRequest(string Email, string RefreshToken);