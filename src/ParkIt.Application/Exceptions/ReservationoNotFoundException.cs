using ParkIt.Core.Exceptions;

namespace ParkIt.Application.Exceptions;

public sealed class ReservationNotFoundException : ParkItException
{
    public ReservationNotFoundException() : base("Reservation not found.") { }
    public ReservationNotFoundException(string reservationId) : base($"Reservation not found for Id: {reservationId}.")
    {
    }
}
