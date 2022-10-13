namespace ParkIt.Application.DTOs;

public record Tokens
{
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
    public DateTime RefreshExpires { get; set; }
}