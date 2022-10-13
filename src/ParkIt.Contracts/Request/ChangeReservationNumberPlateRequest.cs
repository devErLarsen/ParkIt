namespace ParkIt.Contracts.Request;

public record ChangeReservationNumberPlateRequest(Guid ParkingSpotId, Guid ReservationId, string NumberPlate);