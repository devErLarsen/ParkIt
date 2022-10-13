namespace ParkIt.Contracts.Response;

public record ReservationResponse(
    Guid Id,
    // Guid EmployeeId,
    EmployeeResponse Employee,
    VehicleResponse Vehicle,
    // string ReservationPeriod
    DateTime Start,
    DateTime End
);