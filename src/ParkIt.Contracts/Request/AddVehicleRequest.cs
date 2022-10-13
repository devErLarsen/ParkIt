namespace ParkIt.Contracts.Request;

public record AddVehicleRequest
{

    public string? Type { get; set; }
    public string? NumberPlate { get; set; }
    public Guid EmployeeId { get; set; }

}