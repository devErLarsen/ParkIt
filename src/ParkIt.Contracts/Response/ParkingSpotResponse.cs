namespace ParkIt.Contracts.Response;

public record ParkingSpotResponse(
    string Type,
    string Id,
    string CodeName,
    List<ReservationResponse> Reservations
);