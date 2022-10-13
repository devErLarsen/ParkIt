namespace ParkIt.Contracts.Request;

public record DeleteReservationRequest(Guid ParkingSpotId, Guid ReservationId, Guid EmployeeId);