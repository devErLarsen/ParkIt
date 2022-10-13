using ParkIt.Contracts.Request;
using ParkIt.Contracts.Response;

namespace ParkIt.Application.Services;

public interface IReservationService
{
    Task<ReservationResponse> ReserveParkingSpot(ReserveParkingSpotRequest request);
    Task ChangeNumberPlate(ChangeReservationNumberPlateRequest request);
    Task DeleteReservation(DeleteReservationRequest request);
}