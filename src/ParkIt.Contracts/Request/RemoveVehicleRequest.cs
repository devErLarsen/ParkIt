namespace ParkIt.Contracts.Request;

public record RemoveVehicleRequest(Guid EmployeeId, string NumberPlate);