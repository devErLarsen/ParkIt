namespace ParkIt.Contracts.Request;

public record ReserveParkingSpotRequest()
{
    public Guid ParkingSpotId { get; set; }
    public Guid EmployeeId { get; set; }
    public string? NumberPlate { get; set; }
    public DateTime From { get; set; }
    public DateTime To { get; set; }
}