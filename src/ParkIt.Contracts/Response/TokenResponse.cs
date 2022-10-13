namespace ParkIt.Contracts.Response;

public record TokenResponse()
{
    public string? AccessToken { get; set; }
}